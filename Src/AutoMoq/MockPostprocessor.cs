using System;
using Moq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Post-processes a <see cref="Mock{T}"/> instance by setting appropriate default behavioral
    /// values.
    /// </summary>
    public class MockPostprocessor : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockPostprocessor"/> class with the
        /// supplied <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="builder">
        /// The builder which is expected to create <see cref="Mock{T}"/> instances.
        /// </param>
        public MockPostprocessor(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the builder decorated by this instance.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Modifies a <see cref="Mock{T}"/> instance created by <see cref="Builder"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by <see cref="Builder"/>. If the instance is a correct
        /// <see cref="Mock{T}"/> instance, this instance modifies it before returning it.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (!t.IsMock())
            {
                return new NoSpecimen();
            }

            var specimen = this.builder.Create(request, context);
            if (specimen is NoSpecimen || specimen is OmitSpecimen || specimen == null)
                return specimen;

            var m = specimen as Mock;
            if (m == null)
            {
                return new NoSpecimen();
            }

            var mockType = t.GetMockedType();
            if (m.GetType().GetMockedType() != mockType)
            {
                return new NoSpecimen();
            }

            var configurator = (IMockConfigurator)Activator.CreateInstance(typeof(MockConfigurator<>).MakeGenericType(mockType));
            configurator.Configure(m);

            return m;
        }

        private class MockConfigurator<T> : IMockConfigurator where T : class
        {
            public void Configure(Mock mock)
            {
                MockConfigurator<T>.Configure((Mock<T>)mock);
            }

            private static void Configure(Mock<T> mock)
            {
                mock.CallBase = true;
                mock.DefaultValue = DefaultValue.Mock;
            }
        }

        private interface IMockConfigurator
        {
            void Configure(Mock mock);
        }
    }
}
