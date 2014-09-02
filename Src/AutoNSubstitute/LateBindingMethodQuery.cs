using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Selects public static methods that has the same parameters of another method,
    /// ignoring optional parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The main target of this <see cref="IMethodQuery" /> implementation is to be able to
    /// late bind to a method even if it has optional parameters added to it.
    /// </para>
    /// <para>
    /// The order of the methods are based on the match of parameters types of both methods,
    /// favoring the method with exactly same parameters to be returned first.
    /// </para>
    /// </remarks>
    public class LateBindingMethodQuery : IMethodQuery
    {
        private readonly MethodInfo signature;

        /// <summary>
        /// Initializes a new instance of the <see cref="LateBindingMethodQuery"/> class.
        /// </summary>
        /// <param name="signature">The method info to compare.</param>
        public LateBindingMethodQuery(MethodInfo signature)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            this.signature = signature;
        }

        public MethodInfo Signature
        {
            get { return signature; }
        }

        /// <summary>
        /// Selects the methods for the supplied type similar to <see cref="Signature" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public static methods for <paramref name="type"/>, ordered by the most similar first.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned methods is based on a score that matches the parameters types
        /// of the method with the <see cref="Signature" /> method parameters. Methods with the highest score
        /// are returned before.
        /// </para>
        /// <para>
        /// The score is calculated with the following rules: The methods earns 100 points for each exact
        /// match parameter type, it loses 50 points for each hierarchy level down of non-matching parameter
        /// type and it loses 1 point for each optional parameter. It also sums the score for the generic type
        /// arguments or element type with a 10% weight and will decrease 50 points for the difference between
        /// the length of the type arguments array. It also gives 5 points if the type is a class instead of
        /// an interface.
        /// </para>
        /// <para>
        /// In case of two methods with an equal score, the ordering is unspecified.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                   where method.Name == Signature.Name
                   let methodParameters = method.GetParameters()
                   let signatureParameters = Signature.GetParameters()
                   where methodParameters.Length >= signatureParameters.Length
                   let score = new LateBindingParameterScore(methodParameters, signatureParameters)
                   orderby score descending
                   where methodParameters.All(p =>
                       p.Position >= signatureParameters.Length ?
                           p.IsOptional || p.IsDefined(typeof(ParamArrayAttribute), true) :
                           Compare(p.ParameterType, signatureParameters[p.Position].ParameterType))
                   select new LateBoundMethod(new GenericStaticMethod(method));
        }

        private bool Compare(Type parameterType, Type signatureParameterType)
        {
            if (parameterType == signatureParameterType)
                return true;

            if (parameterType.IsAssignableFrom(signatureParameterType))
                return true;

            if (parameterType.IsGenericParameter)
                return signatureParameterType.IsGenericParameter && parameterType.GenericParameterPosition == signatureParameterType.GenericParameterPosition;

            if (parameterType.HasElementType && signatureParameterType.HasElementType)
                return Compare(parameterType.GetElementType(), signatureParameterType.GetElementType());

            var genericArguments = parameterType.GetGenericArguments();
            var signatureGenericArguments = signatureParameterType.GetGenericArguments();
            if (genericArguments.Length == 0 || genericArguments.Length != signatureGenericArguments.Length)
                return false;

            return genericArguments.Zip(signatureGenericArguments, Compare).All(x => x);
        }

        private class LateBindingParameterScore : IComparable<LateBindingParameterScore>
        {
            private readonly int score;

            internal LateBindingParameterScore(IEnumerable<ParameterInfo> methodParameters, IEnumerable<ParameterInfo> signatureParameters)
            {
                if (methodParameters == null)
                    throw new ArgumentNullException("methodParameters");

                if (signatureParameters == null)
                    throw new ArgumentNullException("signatureParameters");

                this.score = CalculateScore(methodParameters, signatureParameters);
            }

            public int CompareTo(LateBindingParameterScore other)
            {
                if (other == null)
                    return 1;

                return this.score.CompareTo(other.score);
            }

            private static int CalculateScore(IEnumerable<ParameterInfo> methodParameters, IEnumerable<ParameterInfo> signatureParameters)
            {
                var parametersScore = signatureParameters.Zip(methodParameters,
                    (s, m) => CalculateScore(m.ParameterType, s.ParameterType))
                    .Sum();

                var additionalParameters = methodParameters.Count() - signatureParameters.Count();

                return parametersScore - additionalParameters;
            }

            private static int CalculateScore(Type methodParameterType, Type signatureParameterType)
            {
                if (methodParameterType == signatureParameterType)
                    return 100;

                var hierarchy = GetHierarchy(signatureParameterType).ToList();
                
                var matches = methodParameterType.IsClass ? 
                    hierarchy.Count(t => t.IsAssignableFrom(methodParameterType)) : 
                    hierarchy.Count(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(methodParameterType)));

                var score = 50 * -(hierarchy.Count - matches);

                var methodTypeArguments = methodParameterType.HasElementType ? new[] { methodParameterType.GetElementType() } : methodParameterType.GetGenericArguments();
                var signatureTypeArguments = signatureParameterType.HasElementType ? new[] {signatureParameterType.GetElementType()} : signatureParameterType.GetGenericArguments();

                score += methodTypeArguments.Zip(signatureTypeArguments, CalculateScore).Sum() / 10;
                score += 50 * -Math.Abs(methodTypeArguments.Length - signatureTypeArguments.Length);

                if (methodParameterType.IsClass)
                    score += 5;
                
                return score;
            }

            private static IEnumerable<Type> GetHierarchy(Type type)
            {
                if (!type.IsClass)
                    foreach (var interfaceType in type.GetInterfaces())
                        yield return interfaceType;

                while (type != null)
                {
                    yield return type;
                    type = type.BaseType;
                }
            }
        }
    }

}