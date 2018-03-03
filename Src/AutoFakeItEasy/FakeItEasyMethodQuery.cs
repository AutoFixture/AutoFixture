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
            if (!type.IsFake())
            {
                return Enumerable.Empty<IMethod>();
            }

            var fakeType = type.GetFakedType();
            if (fakeType.GetTypeInfo().IsInterface || fakeType.IsDelegate())
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
        {
            public FakeMethod(IEnumerable<ParameterInfo> parameterInfos)
            {
                this.Parameters = parameterInfos;
            }

            public IEnumerable<ParameterInfo> Parameters { get; }

            public object Invoke(IEnumerable<object> parameters)
            {
                var genericFakeType = typeof (Fake<>).MakeGenericType(typeof (T));

                foreach (var constructor in genericFakeType.GetConstructors())
                {
                    var constructorParameterInfos = constructor.GetParameters();
                    if (constructorParameterInfos.Length != 1)
                    {
                        continue;
                    }

                    var parameterType = constructorParameterInfos[0].ParameterType;
                    if (!parameterType.GetTypeInfo().IsGenericType ||
                        parameterType.GetGenericTypeDefinition() != typeof (Action<>))
                    {
                        continue;
                    }

                    // The parameter is an action of type
                    // Action<IFakeOptionsBuilder<T>> (FakeItEasy 1.x) or
                    // Action<IFakeOptions<T>> (FakeItEasy 2.0+).
                    // Each of the options-type interfaces contains a WithArgumentsForConstructor method
                    // that we'll use to pass arguments to the fake object's constructor.
                    var fakeOptionsType = parameterType.GetGenericArguments()[0];

                    var withArgumentsForConstructorMethod = fakeOptionsType.GetMethod(
                        "WithArgumentsForConstructor",
                        new[] {typeof (object[])});

                    if (withArgumentsForConstructorMethod == null)
                    {
                        continue;
                    }

                    Action<object> addConstructorArgumentsToOptionsAction =
                        options => withArgumentsForConstructorMethod.Invoke(options, new object[] {parameters});

                    return constructor.Invoke(new object[] {addConstructorArgumentsToOptionsAction});
                }

                return null;
            }
        }
    }
}
