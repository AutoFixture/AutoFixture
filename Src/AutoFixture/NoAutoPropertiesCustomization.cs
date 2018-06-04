using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that will turn off the auto population of properties on the target type.
    /// </summary>
    public class NoAutoPropertiesCustomization : ICustomization
    {
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoAutoPropertiesCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to disable auto population of properties.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> is null.
        /// </exception>
        public NoAutoPropertiesCustomization(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            this.targetType = targetType;
        }

        /// <summary>
        /// Customizes the fixture by creating a <see cref="targetType"/> that has no auto populated properties.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fixture"/> is null.
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            var constructor = new MethodInvoker(new ModestConstructorQuery());

            var builder = SpecimenBuilderNodeFactory.CreateTypedNode(
                this.targetType, constructor);

            fixture.Customizations.Insert(0, builder);
        }
    }
}
