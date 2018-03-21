using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using NSubstitute;

namespace AutoFixture.AutoNSubstitute
{
    /// <summary>Selects appropriate methods to create substitutes.</summary>
    public class NSubstituteMethodQuery : IMethodQuery
    {
        private static readonly IRequestSpecification DelegateSpecification = new DelegateSpecification();

        /// <summary>Selects the methods for the supplied type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.GetTypeInfo().IsInterface || DelegateSpecification.IsSatisfiedBy(type))
                return new[] { SubstituteMethod.Create(type) };

            return from ci in type.GetPublicAndProtectedConstructors()
                   let parameters = ci.GetParameters()
                   orderby parameters.Length ascending
                   select SubstituteMethod.Create(type, parameters);
        }

        private static class SubstituteMethod
        {
            public static IMethod Create(Type type)
            {
                return Create(type, new ParameterInfo[0]);
            }

            public static IMethod Create(
                Type type,
                IEnumerable<ParameterInfo> parameterInfos)
            {
                var constructedType = typeof(SubstituteMethod<>).MakeGenericType(type);
                return (IMethod)Activator.CreateInstance(
                    constructedType,
                    parameterInfos);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", 
            Justification = "It's activated via reflection.")]
        private class SubstituteMethod<T> : IMethod where T : class
        {
            public SubstituteMethod(IEnumerable<ParameterInfo> parameterInfos)
            {
                this.Parameters = parameterInfos;
            }

            public IEnumerable<ParameterInfo> Parameters { get; }

            public object Invoke(IEnumerable<object> parameters)
            {
                return Substitute.For<T>(parameters.ToArray());
            }
        }

    }
}
