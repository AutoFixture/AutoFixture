namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Composes instances of <see cref="ISpecimenBuilder"/>.
    /// </summary>
#warning Consider whether this becomes redundant
    public interface ISpecimenBuilderComposer
    {
        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>A new <see cref="ISpecimenBuilder"/> instance.</returns>
        ISpecimenBuilder Compose();
    }
}
