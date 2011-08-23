namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Composes instances of <see cref="ISpecimenBuilder"/>.
    /// </summary>
    public interface ISpecimenBuilderComposer
    {
        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>A new <see cref="ISpecimenBuilder"/> instance.</returns>
        ISpecimenBuilder Compose();
    }
}
