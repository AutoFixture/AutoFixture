using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// A customization that will freeze a specimen of a given <see cref="Type"/>.
    /// </summary>
    public class FreezingCustomization : ICustomization
    {
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezingCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        public FreezingCustomization(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.targetType = targetType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to freeze.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
        }

        #region ICustomization Members

        /// <summary>
        /// Customizes the fixture by freezing the value of <see cref="TargetType"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            var specimen = new SpecimenContext(fixture.Compose()).Resolve(this.TargetType);

            var fixedFactory = new FixedBuilder(specimen);
            var composer = new TypedBuilderComposer(this.TargetType, fixedFactory);

            fixture.Customizations.Insert(0, composer.Compose());
        }

        #endregion
    }
}
