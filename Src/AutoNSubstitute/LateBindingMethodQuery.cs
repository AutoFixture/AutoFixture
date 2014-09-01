using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class LateBindingMethodQuery : IMethodQuery
    {
        private readonly MethodInfo signature;

        public LateBindingMethodQuery(MethodInfo signature)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            this.signature = signature;
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                   where method.Name == signature.Name
                   let methodParameters = method.GetParameters()
                   let signatureParameters = signature.GetParameters()
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
                    (s, m) => CalculateScore(m.ParameterType, s.ParameterType) * 1000)
                    .Sum();

                var additionalParameters = methodParameters.Count() - signatureParameters.Count();

                return parametersScore - additionalParameters;
            }

            private static int CalculateScore(Type methodParameterType, Type signatureParameterType)
            {
                if (methodParameterType == signatureParameterType)
                    return 1;

                var hierarchy = GetHierarchy(signatureParameterType).ToList();

                var matches = methodParameterType.IsClass ? 
                    hierarchy.Count(t => t.IsAssignableFrom(methodParameterType)) : 
                    hierarchy.Count(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(methodParameterType)));

                return -(hierarchy.Count - matches);
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