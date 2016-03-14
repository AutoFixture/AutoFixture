using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Selects public static methods that has the same parameters of another method,
    /// ignoring optional parameters and return type.
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
    public class TemplateMethodQuery : IMethodQuery
    {
        private readonly MethodInfo template;
        private readonly object owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMethodQuery"/> class.
        /// </summary>
        /// <param name="template">The method info to compare.</param>
        public TemplateMethodQuery(MethodInfo template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            this.template = template;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMethodQuery"/> class.
        /// </summary>
        /// <param name="template">The method info to compare.</param>
        /// <param name="owner">The owner.</param>
        public TemplateMethodQuery(MethodInfo template, object owner)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            this.owner = owner;
            this.template = template;
        }

        /// <summary>
        /// The template <see cref="MethodInfo" /> to compare.
        /// </summary>
        public MethodInfo Template
        {
            get { return template; }
        }

        /// <summary>
        /// The owner instance of the <see cref="MethodInfo" />.
        /// </summary>
        public object Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// Selects the methods for the supplied type similar to <see cref="Template" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public static methods for <paramref name="type"/>, ordered by the most similar first.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned methods is based on a score that matches the parameters types
        /// of the method with the <see cref="Template" /> method parameters. Methods with the highest score
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
                throw new ArgumentNullException(nameof(type));

            return from method in type.GetMethods()
                   where method.Name == Template.Name && (Owner != null || method.IsStatic)
                   let methodParameters = method.GetParameters()
                   let templateParameters = Template.GetParameters()
                   where methodParameters.Length >= templateParameters.Length
                   let score = new LateBindingParameterScore(methodParameters, templateParameters)
                   orderby score descending
                   where methodParameters.All(p =>
                       p.Position >= templateParameters.Length ?
                           p.IsOptional || p.IsDefined(typeof(ParamArrayAttribute), true) :
                           Compare(p.ParameterType, templateParameters[p.Position].ParameterType))
                   select new GenericMethod(method, GetMethodFactory(method));
        }

        private IMethodFactory GetMethodFactory(MethodInfo method)
        {
            if (method.IsStatic)
                return new MissingParametersSupplyingStaticMethodFactory();

            return new MissingParametersSupplyingMethodFactory(Owner);
        }

        private bool Compare(Type parameterType, Type templateParameterType)
        {
            if (parameterType.IsAssignableFrom(templateParameterType))
                return true;

            if (parameterType.IsGenericParameter)
                return templateParameterType.IsGenericParameter
                    && parameterType.GenericParameterPosition == templateParameterType.GenericParameterPosition;

            var genericArguments = GetTypeArguments(parameterType);
            var templateGenericArguments = GetTypeArguments(templateParameterType);

            if (genericArguments.Length == 0 || genericArguments.Length != templateGenericArguments.Length)
                return false;

            return genericArguments.Zip(templateGenericArguments, Compare).All(x => x);
        }

        private static Type[] GetTypeArguments(Type type)
        {
            return type.HasElementType ?
                new[] { type.GetElementType() } :
                type.GetGenericArguments();
        }

        private class LateBindingParameterScore : IComparable<LateBindingParameterScore>
        {
            private readonly int score;

            internal LateBindingParameterScore(IEnumerable<ParameterInfo> methodParameters, 
                IEnumerable<ParameterInfo> templateParameters)
            {
                if (methodParameters == null)
                    throw new ArgumentNullException(nameof(methodParameters));

                if (templateParameters == null)
                    throw new ArgumentNullException(nameof(templateParameters));

                this.score = CalculateScore(methodParameters.Select(p => p.ParameterType), 
                    templateParameters.Select(p => p.ParameterType));
            }

            public int CompareTo(LateBindingParameterScore other)
            {
                if (other == null)
                    return 1;

                return this.score.CompareTo(other.score);
            }

            private static int CalculateScore(IEnumerable<Type> methodParameters, 
                IEnumerable<Type> templateParameters)
            {
                var parametersScore = templateParameters.Zip(methodParameters,
                    (s, m) => CalculateScore(m, s))
                    .Sum();

                var additionalParameters = methodParameters.Count() - templateParameters.Count();

                return parametersScore - additionalParameters;
            }

            private static int CalculateScore(Type methodParameterType, Type templateParameterType)
            {
                if (methodParameterType == templateParameterType)
                    return 100;

                var hierarchy = GetHierarchy(templateParameterType).ToList();
                
                var matches = methodParameterType.IsClass ? 
                    hierarchy.Count(t => t.IsAssignableFrom(methodParameterType)) : 
                    hierarchy.Count(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(methodParameterType)));

                var score = 50 * -(hierarchy.Count - matches);

                var methodTypeArguments = GetTypeArguments(methodParameterType);
                var templateTypeArguments = GetTypeArguments(templateParameterType);

                score += CalculateScore(methodTypeArguments, templateTypeArguments) / 10;
                score += 50 * -Math.Abs(methodTypeArguments.Length - templateTypeArguments.Length);

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