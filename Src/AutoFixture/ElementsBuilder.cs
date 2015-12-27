using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Draws a random element from the given collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ElementsBuilder<T> : ISpecimenBuilder
    {
        private readonly ICollection<T> elements;
        private readonly RandomNumericSequenceGenerator sequence;

        private readonly object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementsBuilder"/> class.
        /// </summary>
        /// <param name="elements">The collection from which elements should be drawn from.</param>
        public ElementsBuilder(ICollection<T> elements)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            if (elements.Count < 2)
                throw new ArgumentException("The collection should at least hold two elements.", nameof(elements));

            this.elements = elements;
            this.sequence = new RandomNumericSequenceGenerator(0, elements.Count - 1);
        }

        /// <summary>
        /// Returns one of the element present in the collection given when the object was constructed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// One of the element present in the collection given to the constructor if <paramref name="request"/>
        /// is a request for <typeparamref name="T"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(T).Equals(request))
                return new NoSpecimen();

            return this.elements.ElementAt(this.GetNext());
        }

        private int GetNext()
        {
            lock (this.syncRoot)
            {
                var i = (int)this.sequence.Create(typeof(int));
                return i;
            }
        }
    }
}
