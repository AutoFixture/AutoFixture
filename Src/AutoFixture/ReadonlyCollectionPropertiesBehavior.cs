using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Decorates <see cref="ISpecimenBuilder"/> with a <see cref="Postprocessor"/> which invokes
    /// <see cref="ReadonlyCollectionPropertiesCommand"/> if the specimen meets the
    /// <see cref="ReadonlyCollectionPropertiesSpecification"/>.
    /// </summary>
    public class ReadonlyCollectionPropertiesBehavior : ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Constructs an instance of <see cref="ReadonlyCollectionPropertiesBehavior"/>, used to decorate an
        /// <see cref="ISpecimenBuilder"/> with a <see cref="Postprocessor"/> which invokes
        /// <see cref="ReadonlyCollectionPropertiesCommand"/> if the specimen meets the
        /// <see cref="ReadonlyCollectionPropertiesSpecification"/>.
        /// </summary>
        public ReadonlyCollectionPropertiesBehavior()
            : this(ReadonlyCollectionPropertiesSpecification.DefaultPropertyQuery)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="ReadonlyCollectionPropertiesBehavior"/>, used to decorate an
        /// <see cref="ISpecimenBuilder"/> with a <see cref="Postprocessor"/> which invokes
        /// <see cref="ReadonlyCollectionPropertiesCommand"/> if the specimen meets the
        /// <see cref="ReadonlyCollectionPropertiesSpecification"/>.
        /// </summary>
        /// <param name="propertyQuery">The query that will be applied to select readonly collection properties.</param>
        public ReadonlyCollectionPropertiesBehavior(IPropertyQuery propertyQuery)
        {
            this.PropertyQuery = propertyQuery;
        }

        /// <summary>
        /// Gets the query used to determine whether or not a specified type has readonly collection properties.
        /// </summary>
        public IPropertyQuery PropertyQuery { get; }

        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder"/> with a <see cref="Postprocessor"/> which invokes
        /// <see cref="ReadonlyCollectionPropertiesCommand"/> if the specimen meets the
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
                new ReadonlyCollectionPropertiesCommand(this.PropertyQuery),
                new AndRequestSpecification(
                    new ReadonlyCollectionPropertiesSpecification(this.PropertyQuery),
                    new OmitFixtureSpecification()));
        }
    }
}