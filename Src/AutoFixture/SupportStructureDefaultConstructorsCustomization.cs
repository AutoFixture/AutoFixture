using System;

namespace Ploeh.AutoFixture

{
    /// <summary>
    /// A customization that changes how custom <see langword="struct"/> are generated. Uses <see cref="SupportStructureDefaultConstructorsGenerator"/>.
    /// </summary>
    public class SupportStructureDefaultConstructorsCustomization : ICustomization
    {
        /// <summary>
        /// Customizes specified fixture by adding <see cref="SupportStructureDefaultConstructorsGenerator"/> as a default stragety for 
        /// creating new custom <see langword="struct"/> that has only default constructor.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new SupportStructureDefaultConstructorsGenerator());
        }
    }
}