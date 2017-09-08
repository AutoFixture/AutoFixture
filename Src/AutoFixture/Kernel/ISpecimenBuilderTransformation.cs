namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Transforms one <see cref="ISpecimenBuilder"/> instance into another.
    /// </summary>
    public interface ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Transforms the supplied builder into another.
        /// </summary>
        /// <param name="builder">The builder to transform.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> created from <paramref name="builder"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Note to implementers: In most scenarios, the transformation is expected to maintain
        /// behavior of <paramref name="builder"/>; usually by applying a Decorator.
        /// </para>
        /// </remarks>
        ISpecimenBuilder Transform(ISpecimenBuilder builder);
    }
}
