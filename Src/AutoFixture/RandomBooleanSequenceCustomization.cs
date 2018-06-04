using System;

namespace AutoFixture
{
    /// <summary>
    /// A customization that changes how <see cref="bool"/> are generated. Uses <see cref="RandomBooleanSequenceGenerator"/>.
    /// </summary>
    public class RandomBooleanSequenceCustomization : ICustomization
    {
        /// <summary>
        /// Customizes specified fixture by adding <see cref="RandomBooleanSequenceGenerator"/> as a default strategy for
        /// creating new <see cref="bool"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(new RandomBooleanSequenceGenerator());
        }
    }
}