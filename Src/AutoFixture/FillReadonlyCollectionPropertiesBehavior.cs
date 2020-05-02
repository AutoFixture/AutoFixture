using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Decorates <see cref="ISpecimenBuilder"/> with a <see cref="Postprocessor"/> which invokes
    /// <see cref="FillReadonlyCollectionPropertiesCommand"/> if the specimen meets the
    /// <see cref="ReadonlyCollectionPropertiesSpecification"/>.
    /// </summary>
    public class FillReadonlyCollectionPropertiesBehavior : ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder"/> with a <see cref="Postprocessor"/> which invokes
        /// <see cref="FillReadonlyCollectionPropertiesCommand"/> if the specimen meets the
        /// <see cref="ReadonlyCollectionPropertiesSpecification"/>.
        /// </summary>
        /// <param name="builder">
        /// The builder to decorate.
        /// </param>
        /// <returns>
        /// <paramref name="builder"/> decorated with a <see cref="Postprocessor"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return new Postprocessor(
                builder,
                new FillReadonlyCollectionPropertiesCommand(),
                new ReadonlyCollectionPropertiesSpecification());
        }
    }
}