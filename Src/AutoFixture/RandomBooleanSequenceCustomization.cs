using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that changes how <see cref="Boolean"/> are generated. Uses <see cref="RandomBooleanSequenceGenerator"/>.
    /// </summary>
    public class RandomBooleanSequenceCustomization : ICustomization
    {
        /// <summary>
        /// Customizes specified fixture by adding <see cref="RandomBooleanSequenceGenerator"/> as a default strategy for 
        /// creating new <see cref="Boolean"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new RandomBooleanSequenceGenerator());
        }
    }
}