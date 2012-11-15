using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates specimens by returning the first specimen created by its children.
    /// </summary>
    public class CompositeSpecimenBuilder : ISpecimenBuilderNode
    {
        private readonly List<ISpecimenBuilder> builders;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecimenBuilder"/> class.
        /// </summary>
#warning Remove when Builders are changed to IEnumerable<ISpecimenBuilder>
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
#warning This should be an IEnumerable<ISpecimenBuilder>
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
                    select result).DefaultIfEmpty(new NoSpecimen(request)).FirstOrDefault();
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new CompositeSpecimenBuilder(builders);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through
        /// the collection.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            return this.builders.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can
        /// be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal static ISpecimenBuilder ComposeIfMultiple(IEnumerable<ISpecimenBuilder> builders)
        {
            var isSingle = builders.Take(2).Count() == 1;
            if (isSingle)
                return builders.Single();

            return new CompositeSpecimenBuilder(builders);
        }

        internal static ISpecimenBuilderNode UnwrapIfSingle(ISpecimenBuilderNode node)
        {
            var c = node as CompositeSpecimenBuilder;
            if (c == null)
                return node;
            var isSingle = c.builders.Take(2).Count() == 1;
            if (isSingle)
            {
                var n = c.builders.Single() as ISpecimenBuilderNode;
                if (n != null)
                    return n;
            }
            return node;
        }
    }
}
