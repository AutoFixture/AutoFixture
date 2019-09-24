using System;
using System.Reflection;
using AutoFixture.AutoMoq.Extensions;
using AutoFixture.Kernel;
using Moq;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking with Moq.
    /// </summary>
    public class AutoMoqCustomization : ICustomization
    {
        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with Moq.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        /// <exception cref="ArgumentNullException">When the fixture is null.</exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            fixture.Customizations.Add(new MockPostprocessor(new MethodInvoker(new MockConstructorQuery())));
            fixture.Customizations.Add(new AutoMoqTypeRelay());
        }

        private class AutoMoqTypeRelay : ISpecimenBuilder
        {
            private static readonly IRequestSpecification Specification = new MockableSpecification();

            public object Create(object request, ISpecimenContext context)
            {
                if (!Specification.IsSatisfiedBy(request))
                    return new NoSpecimen();

                var mockType = typeof(Mock<>).MakeGenericType((Type)request);
                var result = context.Resolve(mockType) as Mock;

                // The order here is important to enable both fixture values and storing the value passed to a setter.
                result.DefaultValueProvider = new AutoFixtureValueProvider(context);
                result.GetType().GetMethod("SetupAllProperties").Invoke(result, new object[0]);

                return result.Object;
            }
        }

        private class AutoFixtureValueProvider : DefaultValueProvider
        {
            private readonly ISpecimenContext context;

            public AutoFixtureValueProvider(ISpecimenContext context)
            {
                this.context = context;
            }

            protected override object GetDefaultValue(Type type, Mock mock) => this.context.Resolve(type);
        }

        private class MockableSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                var type = request as Type;
                if (type == null)
                    return false;

                var info = type.GetTypeInfo();
                if (!info.IsInterface && !info.IsAbstract && !type.IsDelegate())
                    return false;

                return type != typeof(DefaultValueProvider);
            }
        }
    }
}
