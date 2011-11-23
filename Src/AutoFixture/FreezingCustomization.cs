using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that will freeze a specimen of a given <see cref="Type"/>.
    /// </summary>
    public class FreezingCustomization : ICustomization
    {
        private readonly Type targetType;
        private readonly Type registeredType;
        private IFixture currentFixture;
        private FixedBuilder fixedSpecimenBuilder;

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
        public FreezingCustomization(Type targetType, Type registeredType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (registeredType == null)
            {
                throw new ArgumentNullException("registeredType");
            }

            this.targetType = targetType;
            this.registeredType = registeredType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to freeze.
        /// </summary>
        public Type TargetType
        {
            get { return targetType; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to which the frozen <see cref="TargetType"/> value
        /// should be mapped to. Defaults to the same <see cref="Type"/> as <see cref="TargetType"/>.
        /// </summary>
        public Type RegisteredType
        {
            get { return registeredType; }
        }

        /// <summary>
        /// Customizes the fixture by freezing the value of <see cref="TargetType"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fixture"/> is null.
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            currentFixture = fixture;
            CreateFixedSpecimenBuilderForTargetType();
            RegisterFixedSpecimenBuilderForTargetTypeAndRegisteredType();
        }

        private void CreateFixedSpecimenBuilderForTargetType()
        {
            var specimen = CreateSpecimenForTargetType();
            fixedSpecimenBuilder = new FixedBuilder(specimen);
        }

        private object CreateSpecimenForTargetType()
        {
            var context = new SpecimenContext(currentFixture.Compose());
            return context.Resolve(targetType);
        }

        private void RegisterFixedSpecimenBuilderForTargetTypeAndRegisteredType()
        {
            var targetTypeBuilder = MapFixedSpecimenBuilderToTargetType();
            var registeredTypeBuilder = MapFixedSpecimenBuilderToRegisteredType();

            var compositeBuilder = new CompositeSpecimenBuilder(
                targetTypeBuilder,
                registeredTypeBuilder);

            currentFixture.Customizations.Insert(0, compositeBuilder);
        }

        private ISpecimenBuilder MapFixedSpecimenBuilderToTargetType()
        {
            var builderComposer = new TypedBuilderComposer(targetType, fixedSpecimenBuilder);
            return builderComposer.Compose();
        }

        private ISpecimenBuilder MapFixedSpecimenBuilderToRegisteredType()
        {
            var builderComposer = new TypedBuilderComposer(registeredType, fixedSpecimenBuilder);
            return builderComposer.Compose();
        }
    }
}
