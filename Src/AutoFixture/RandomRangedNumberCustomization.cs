using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that enables numeric specimens to be random and unique per equivalence set of type and range limits.
    /// </summary>
    public class RandomRangedNumberCustomization : ICustomization 
    {
        /// <summary>
        /// Customizes the specified fixture by adding a
        /// <see cref="RandomRangedNumberGenerator" />.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="System.ArgumentNullException">
        /// fixture is null
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(new RandomRangedNumberGenerator());
        }
    }
}
