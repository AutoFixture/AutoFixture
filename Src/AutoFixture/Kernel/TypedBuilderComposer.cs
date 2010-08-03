using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Composes a <see cref="ISpecimenBuilder"/> that targets a particular <see cref="Type"/>.
    /// </summary>
    public class TypedBuilderComposer : ISpecimenBuilderComposer
    {
        private readonly ISpecimenBuilder factory;
        private readonly Type targetType;
        private readonly IRequestSpecification inputFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedBuilderComposer"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> targeted by the <see cref="ISpecimenBuilder"/> created by
        /// <see cref="Compose"/>.
        /// </param>
        /// <param name="factory">
        /// The factory that creates instances of <see cref="TargetType"/>.
        /// </param>
        public TypedBuilderComposer(Type targetType, ISpecimenBuilder factory)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.targetType = targetType;
            this.factory = factory;

            this.inputFilter = new OrRequestSpecification(
                new SeedRequestSpecification(this.TargetType),
                new ExactTypeSpecification(this.TargetType));
        }

        /// <summary>
        /// Gets the factory that creates instance of <see cref="TargetType"/>.
        /// </summary>
        public ISpecimenBuilder Factory
        {
            get { return this.factory; }
        }

        /// <summary>
        /// Gets the targeted <see cref="Type"/>.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
        }

        #region ISpecimenBuilderComposer Members

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> instance with appropriate filters that targets
        /// <see cref="TargetType"/>.
        /// </returns>
        public ISpecimenBuilder Compose()
        {
            return this.Transformations.Aggregate(this.Factory, (b, t) => t.Transform(b));
        }

        #endregion

        /// <summary>
        /// Gets the input filter.
        /// </summary>
        protected IRequestSpecification InputFilter
        {
            get { return this.inputFilter; }
        }

        /// <summary>
        /// Gets the transformations that will be applied to <see cref="Factory"/> during
        /// <see cref="Compose"/>.
        /// </summary>
        protected virtual IEnumerable<ISpecimenBuilderTransformation> Transformations
        {
            get
            {
                yield return new DecorateWithOutputGuardTransformation(this.TargetType);
                yield return new CombineWithSeedRelayTransformation();
                yield return new DecorateWithInputFilterTransformation(this.InputFilter);
            }
        }

        /// <summary>
        /// A tranformation that decorates an <see cref="ISpecimenBuilder"/> with an appropriate
        /// <see cref="NoSpecimenOutputGuard"/>.
        /// </summary>
        protected class DecorateWithOutputGuardTransformation : ISpecimenBuilderTransformation
        {
            private readonly Type targetType;

            /// <summary>
            /// Initializes a new instance of the <see cref="DecorateWithOutputGuardTransformation"/> class.
            /// </summary>
            /// <param name="targetType">The target <see cref="Type"/>.</param>
            public DecorateWithOutputGuardTransformation(Type targetType)
            {
                if (targetType == null)
                {
                    throw new ArgumentNullException("targetType");
                }

                this.targetType = targetType;
            }

            #region ISpecimenBuilderTransformation Members

            /// <summary>
            /// Transforms the supplied builder into another.
            /// </summary>
            /// <param name="builder">The builder to transform.</param>
            /// <returns>
            /// A new <see cref="ISpecimenBuilder"/> created from <paramref name="builder"/>.
            /// </returns>
            /// <remarks>
            /// <para>
            /// Although a <see cref="NoSpecimenOutputGuard"/> throws if the decorated
            /// <see cref="ISpecimenBuilder"/> returns a <see cref="NoSpecimen"/>, this
            /// transformation permits NoSpecimen return values if the request is a
            /// <see cref="SeededRequest"/> for the <see cref="TargetType"/>. This enables the
            /// engine to retry the request with the unwrapped <see cref="Type"/> request.
            /// </para>
            /// </remarks>
            public ISpecimenBuilder Transform(ISpecimenBuilder builder)
            {
                var allowedNoSpecimenFilter = new SeedRequestSpecification(this.targetType);
                var throwSpec = new InverseRequestSpecification(allowedNoSpecimenFilter);
                return new NoSpecimenOutputGuard(builder, throwSpec);
            }

            #endregion
        }

        /// <summary>
        /// A transformation that combines an <see cref="ISpecimenBuilder"/> with a
        /// <see cref="SeedIgnoringRelay"/>.
        /// </summary>
        protected class CombineWithSeedRelayTransformation : ISpecimenBuilderTransformation
        {
            #region ISpecimenBuilderTransformation Members

            /// <summary>
            /// Transforms the supplied builder into another.
            /// </summary>
            /// <param name="builder">The builder to transform.</param>
            /// <returns>
            /// A new <see cref="ISpecimenBuilder"/> created from <paramref name="builder"/>.
            /// </returns>
            /// <remarks>
            /// <para>
            /// This method combines <paramref name="builder"/> with a
            /// <see cref="SeedIgnoringRelay"/>. This ensures that if the original builder cannot
            /// deal with a <see cref="SeededRequest"/> the request is immediately retried with the
            /// unwrapped request.
            /// </para>
            /// </remarks>
            public ISpecimenBuilder Transform(ISpecimenBuilder builder)
            {
                return new CompositeSpecimenBuilder(builder, new SeedIgnoringRelay());
            }

            #endregion
        }

        /// <summary>
        /// A transformation that decorates an <see cref="ISpecimenBuilder"/> with a filter that
        /// filters according to a target type.
        /// </summary>
        protected class DecorateWithInputFilterTransformation : ISpecimenBuilderTransformation
        {
            private readonly IRequestSpecification inputFilter;

            /// <summary>
            /// Initializes a new instance of the <see cref="DecorateWithInputFilterTransformation"/> class.
            /// </summary>
            /// <param name="inputFilter">The input filter.</param>
            public DecorateWithInputFilterTransformation(IRequestSpecification inputFilter)
            {
                if (inputFilter == null)
                {
                    throw new ArgumentNullException("inputFilter");
                }

                this.inputFilter = inputFilter;
            }

            #region ISpecimenBuilderTransformation Members

            /// <summary>
            /// Transforms the supplied builder into another.
            /// </summary>
            /// <param name="builder">The builder to transform.</param>
            /// <returns>
            /// A new <see cref="ISpecimenBuilder"/> created from <paramref name="builder"/>.
            /// </returns>
            public ISpecimenBuilder Transform(ISpecimenBuilder builder)
            {
                return new FilteringSpecimenBuilder(builder, this.inputFilter);
            }

            #endregion
        }

    }
}
