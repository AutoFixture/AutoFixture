using Ploeh.AutoFixture.Kernel;
using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that enables numeric specimens to be random and unique.
    /// </summary>
    public class RandomNumericSequenceCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture by adding a
        /// <see cref="RandomNumericSequenceGenerator" />.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="System.ArgumentNullException">
        /// fixture is null
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new RandomNumericSequenceGenerator());
        }
    }
}