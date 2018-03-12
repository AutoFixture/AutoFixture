using System;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy;

namespace AutoFixture.AutoFakeItEasy
{
    internal class WrapDelegateInFakeBuilder : ISpecimenBuilder
    {
        private readonly IRequestSpecification fakeableSpecification;
        private readonly ISpecimenBuilder builder;

        public WrapDelegateInFakeBuilder(IRequestSpecification delegateSpecification, ISpecimenBuilder builder)
        {
            this.fakeableSpecification = delegateSpecification ?? throw new ArgumentNullException(nameof(delegateSpecification));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type.IsFake())
            {
                var fakedType = type.GetFakedType();
                if (this.fakeableSpecification.IsSatisfiedBy(fakedType))
                {
                    var bareDelegate = builder.Create(fakedType, context);
                    return Wrap(bareDelegate);
                }
            }

            return builder.Create(request, context);
        }

        private static object Wrap(object specimen)
        {
            var genericFakeType = typeof(Fake<>).MakeGenericType(specimen.GetType());

            foreach (var constructor in genericFakeType.GetConstructors())
            {
                var constructorParameterInfos = constructor.GetParameters();
                if (constructorParameterInfos.Length != 1)
                {
                    continue;
                }

                var parameterType = constructorParameterInfos[0].ParameterType;
                if (!parameterType.GetTypeInfo().IsGenericType ||
                    parameterType.GetGenericTypeDefinition() != typeof(Action<>))
                {
                    continue;
                }

                // The parameter is an action of type
                // Action<IFakeOptionsBuilder<T>> (FakeItEasy 1.x) or
                // Action<IFakeOptions<T>> (FakeItEasy 2.0+).
                // Each of the options-type interfaces contains a Wrapping method
                // that we'll use to pass the wrapped item to the fake object's constructor.
                var fakeOptionsType = parameterType.GetGenericArguments()[0];

                var withArgumentsForConstructorMethod = fakeOptionsType.GetMethod(
                    "Wrapping",
                    new[] { specimen.GetType() });

                if (withArgumentsForConstructorMethod == null)
                {
                    continue;
                }

                Action<object> addWrappingToOptionsAction =
                    options => withArgumentsForConstructorMethod.Invoke(options, new[] { specimen });

                return constructor.Invoke(new object[] { addWrappingToOptionsAction });
            }

            return null;
        }
    }
}