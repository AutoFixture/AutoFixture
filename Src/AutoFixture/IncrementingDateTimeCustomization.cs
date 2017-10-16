using System;

namespace AutoFixture
{
    /// <summary>
    /// A customization that enables <see cref="DateTime"/> specimens
    /// to be based on an incrementing sequence of days starting from <see cref="DateTime.Now"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This customization reproduces the generation strategy for <see cref="DateTime"/> specimens
    /// used in AutoFixture up to version 3.0.
    /// </para>
    /// </remarks>
    public class IncrementingDateTimeCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fixture"/> is <see langword="null"/>.
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new StrictlyMonotonicallyIncreasingDateTimeGenerator(DateTime.Now));
        }
    }
}
