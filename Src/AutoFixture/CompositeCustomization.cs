using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Customizes an <see cref="IFixture"/> by using all contained <see cref="Customizations"/>.
    /// </summary>
    public class CompositeCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCustomization"/> class.
        /// </summary>
        /// <param name="customizations">The customizations.</param>
        public CompositeCustomization(IEnumerable<ICustomization> customizations)
            : this(customizations.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCustomization"/> class.
        /// </summary>
        /// <param name="customizations">The customizations.</param>
        public CompositeCustomization(params ICustomization[] customizations)
        {
            if (customizations == null)
            {
                throw new ArgumentNullException(nameof(customizations));
            }

            this.Customizations = customizations;
        }

        /// <summary>
        /// Gets the customizations contained within this instance.
        /// </summary>
        public IEnumerable<ICustomization> Customizations { get; }

        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            foreach (var c in this.Customizations)
            {
                c.Customize(fixture);
            }
        }
    }
}
