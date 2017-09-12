using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy2
{
    /// <summary>
    /// Selects appropriate methods to create <see cref="FakeItEasy.Fake{T}"/> instances.
    /// </summary>
    [Obsolete("The AutoFakeItEasy2 package has been retired; use the AutoFakeItEasy (without the trailing \"2\") package instead. Details: it's turned out that it's possible to enable AutoFakeItEasy to also work with FakeItEasy 2. From version 3.49.1, you should be able to use AutoFakeItEasy with FakeItEasy 2 by adding an assembly binding redirect. This enables us, the maintainers of AutoFixture, to maintain only one code base for FakeItEasy, instead of two. If this causes problems, please create an issue at https://github.com/AutoFixture/AutoFixture/issues. We apologise for any inconvenience this may cause.", true)]
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
            if (fakeType.IsInterface)
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

        private class FakeMethod<T> : IMethod
        {
            private readonly IEnumerable<ParameterInfo> parameterInfos;

            public FakeMethod(IEnumerable<ParameterInfo> parameterInfos)
            {
                this.parameterInfos = parameterInfos;
            }

            public IEnumerable<ParameterInfo> Parameters
            {
                get { return this.parameterInfos; }
            }

            public object Invoke(IEnumerable<object> parameters)
            {
                return new Fake<T>(
                    b => b.WithArgumentsForConstructor(parameters));
            }
        }
    }
}
