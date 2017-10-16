namespace AutoFixture
{
    /// <summary>
    /// Encapsulates a customization of an <see cref="IFixture"/>.
    /// </summary>
    public interface ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        void Customize(IFixture fixture);
    }
}
