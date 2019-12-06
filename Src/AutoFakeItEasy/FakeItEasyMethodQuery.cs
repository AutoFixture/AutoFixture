using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Selects appropriate methods to create <see cref="FakeItEasy.Fake{T}"/> instances.
    /// </summary>
    public class FakeItEasyMethodQuery : IMethodQuery
    {
        private static readonly DelegateSpecification DelegateSpecification = new DelegateSpecification();

        /// <summary>
        /// Selects constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method returns a sequence of <see cref="StaticMethod"/> according to
        /// the public and protected constructors available on <paramref name="type"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (!type.IsFake())
            {
                return Enumerable.Empty<IMethod>();
            }

            var fakeType = type.GetFakedType();
            if (fakeType.GetTypeInfo().IsInterface || DelegateSpecification.IsSatisfiedBy(fakeType))
            {
                return new[] { new ConstructorMethod(type.GetDefaultConstructor()) };
            }

            return from ci in fakeType.GetPublicAndProtectedConstructors()
                   let parameters = ci.GetParameters()
                   orderby parameters.Length ascending
                   select FakeMethod.Create(fakeType, parameters);
        }

        private static class FakeMethod
        {
            public static IMethod Create(
                Type type,
                IEnumerable<ParameterInfo> parameterInfos)
            {
                var constructedType =
                    typeof(FakeMethod<>).MakeGenericType(type);
                return (IMethod)Activator.CreateInstance(
                    constructedType,
                    parameterInfos);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class FakeMethod<T> : IMethod
            where T : class
        {
            public FakeMethod(IEnumerable<ParameterInfo> parameterInfos)
            {
                this.Parameters = parameterInfos;
            }

            public IEnumerable<ParameterInfo> Parameters { get; }

            public object Invoke(IEnumerable<object> parameters) => new Fake<T>(b => b.WithArgumentsForConstructor(parameters));
        }
    }
}
