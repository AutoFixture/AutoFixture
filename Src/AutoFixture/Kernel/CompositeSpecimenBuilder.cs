using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Creates specimens by returning the first specimen created by its children.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class CompositeSpecimenBuilder : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder[] composedBuilders;

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
            this.composedBuilders = builders ?? throw new ArgumentNullException(nameof(builders));
        }

        /// <summary>
        /// Gets the child builders.
        /// </summary>
        public IEnumerable<ISpecimenBuilder> Builders => this.composedBuilders;

        /// <summary>
        /// Creates a new specimen by delegating to <see cref="Builders"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>The first result created by <see cref="Builders"/>.</returns>
        public object Create(object request, ISpecimenContext context)
        {
            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            for (int i = 0; i < this.composedBuilders.Length; i++)
            {
                var result = this.composedBuilders[i].Create(request, context);
                if (!(result is NoSpecimen)) return result;
            }

            return new NoSpecimen();
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
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            return this.composedBuilders.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator" /> object that can
        /// be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal static ISpecimenBuilder ComposeIfMultiple(IEnumerable<ISpecimenBuilder> builders)
        {
            // This code is called very frequently during graph mutation (e.g. customization).
            // It's optimized to not perform unnecessary allocations and do not enumerate builders more than once.
            // Code is written expecting that single builder is the most common case.
            // See more detail here: https://github.com/AutoFixture/AutoFixture/pull/793
            ISpecimenBuilder singleItem = null;
            List<ISpecimenBuilder> multipleItems = null;
            bool hasItems = false;

            using (var enumerator = builders.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    singleItem = enumerator.Current;
                    hasItems = true;

                    while (enumerator.MoveNext())
                    {
                        // We are here only if more than one item is present.
                        // When we create collection for multiple items, we add the already enumerated one as well.
                        if (multipleItems == null)
                        {
                            multipleItems = new List<ISpecimenBuilder> { singleItem };
                        }

                        multipleItems.Add(enumerator.Current);
                    }
                }
            }

            if (!hasItems)
            {
                // It's very rare case and it doesn't make sense to optimize it.
                return new CompositeSpecimenBuilder();
            }

            // Single item is present only, so we return it.
            if (multipleItems == null)
            {
                return singleItem;
            }

            return new CompositeSpecimenBuilder(multipleItems);
        }

        internal static ISpecimenBuilderNode UnwrapIfSingle(ISpecimenBuilderNode node)
        {
            var c = node as CompositeSpecimenBuilder;
            if (c == null)
                return node;
            var isSingle = c.composedBuilders.Length == 1;
            if (isSingle)
            {
                if (c.composedBuilders[0] is ISpecimenBuilderNode n)
                    return n;
            }
            return node;
        }
    }
}
