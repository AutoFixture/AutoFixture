using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>Selects appropriate methods to create substitutes.</summary>
    public class NSubstituteMethodQuery : IMethodQuery
    {
        /// <summary>Selects the methods for the supplied type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsInterface)
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
                return SubstituteMethod.Create(type, new ParameterInfo[0]);
            }

            public static IMethod Create(
                Type type,
                IEnumerable<ParameterInfo> parameterInfos)
            {
                var constructedType =
                    typeof(SubstituteMethod<>).MakeGenericType(type);
                return (IMethod)Activator.CreateInstance(
                    constructedType,
                    parameterInfos);
            }
        }

        private class SubstituteMethod<T> : IMethod where T : class
        {
            private readonly IEnumerable<ParameterInfo> parameterInfos;

            public SubstituteMethod(IEnumerable<ParameterInfo> parameterInfos)
            {
                this.parameterInfos = parameterInfos;
            }

            public IEnumerable<ParameterInfo> Parameters
            {
                get { return this.parameterInfos; }
            }

            public object Invoke(IEnumerable<object> parameters)
            {
                return Substitute.For<T>(parameters.ToArray());
            }
        }

    }
}
