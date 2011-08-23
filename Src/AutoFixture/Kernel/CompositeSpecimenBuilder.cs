using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates specimens by returning the first specimen created by its children.
    /// </summary>
    public class CompositeSpecimenBuilder : ISpecimenBuilder
    {
        private readonly List<ISpecimenBuilder> builders;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecimenBuilder"/> class.
        /// </summary>
        public CompositeSpecimenBuilder()
            : this(Enumerable.Empty<ISpecimenBuilder>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecimenBuilder"/> class with the
        /// supplied builders.
        /// </summary>
        /// <param name="builders">The child builders.</param>
        public CompositeSpecimenBuilder(IEnumerable<ISpecimenBuilder> builders)
            : this(builders.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecimenBuilder"/> class with the
        /// supplied builders.
        /// </summary>
        /// <param name="builders">The child builders.</param>
        public CompositeSpecimenBuilder(params ISpecimenBuilder[] builders)
        {
            if (builders == null)
            {
                throw new ArgumentNullException("builders");
            }

            this.builders = builders.ToList();
        }

        /// <summary>
        /// Gets the child builders.
        /// </summary>
        public IList<ISpecimenBuilder> Builders
        {
            get { return this.builders; }
        }

        /// <summary>
        /// Creates a new specimen by delegating to <see cref="Builders"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>The first result created by <see cref="Builders"/>.</returns>
        public object Create(object request, ISpecimenContext context)
        {
            return (from b in this.Builders
                    let result = b.Create(request, context)
                    where !(result is NoSpecimen)
                    select result).DefaultIfEmpty(new NoSpecimen()).FirstOrDefault();
        }
    }
}
