using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that will freeze a specimen of a given <see cref="Type"/>.
    /// </summary>
    public class FreezingCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreezingCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> is null.
        /// </exception>
        public FreezingCustomization(Type targetType)
            : this(targetType, targetType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezingCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        /// <param name="registeredType">
        /// The <see cref="Type"/> to map the frozen <paramref name="targetType"/> value to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="targetType"/> or <paramref name="registeredType"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="registeredType"/> is not assignable from <paramref name="targetType"/>.
        /// </exception>
        public FreezingCustomization(Type targetType, Type registeredType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (registeredType == null) throw new ArgumentNullException(nameof(registeredType));

            if (!registeredType.GetTypeInfo().IsAssignableFrom(targetType))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "The type '{0}' cannot be frozen as '{1}' because the two types are not compatible.",
                    targetType,
                    registeredType);
                throw new ArgumentException(message);
            }

            this.TargetType = targetType;
            this.RegisteredType = registeredType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to freeze.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> to which the frozen <see cref="TargetType"/> value
        /// should be mapped to. Defaults to the same <see cref="Type"/> as <see cref="TargetType"/>.
        /// </summary>
        public Type RegisteredType { get; }

        /// <summary>
        /// Customizes the fixture by freezing the value of <see cref="TargetType"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fixture"/> is null.
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var specimen = fixture.Create(
                    this.TargetType);
            var fixedBuilder = new FixedBuilder(specimen);

            var types = new[]
                {
                    this.TargetType,
                    this.RegisteredType
                };

            var builder = new CompositeSpecimenBuilder(
                from t in types
                select SpecimenBuilderNodeFactory.CreateTypedNode(
                    t, fixedBuilder) as ISpecimenBuilder);

            fixture.Customizations.Insert(0, builder);
        }
    }
}
