using System;

namespace AutoFixture
{
    /// <summary>
    /// A customization that enables DateTime specimens to be based on the current <see cref="DateTime.Now"/> value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When this customization is added to an <see cref="IFixture"/> instance, requests for DateTime specimens
    /// will be satisfied by returning the current <see cref="DateTime.Now"/> value.
    /// </para>
    /// <para>
    /// This customization reproduces the generation strategy for DateTime specimens used in AutoFixture up to version 2.1.
    /// </para>
    /// </remarks>
    public class CurrentDateTimeCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture by adding the <see cref="Type"/> specific numeric sequence generators.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new CurrentDateTimeGenerator());
        }
    }
}