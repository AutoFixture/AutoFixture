﻿using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Draws a random element from the given collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ElementsBuilder<T> : ISpecimenBuilder
    {
        private readonly T[] elements;
        private readonly RandomNumericSequenceGenerator sequence;

        public ElementsBuilder(params T[] elements) : this(elements.AsEnumerable()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementsBuilder{T}"/> class.
        /// </summary>
        /// <param name="elements">The collection from which elements should be drawn from.
        /// It must contain at least one element.</param>
        public ElementsBuilder(IEnumerable<T> elements)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");

            this.elements = elements.ToArray();

            if (this.elements.Length < 1)
                throw new ArgumentException("The supplied collection of elements must contain at least one element. This collection is expected to contain the elements from which the randomized algorithm will draw; if the collection is empty, there are no elements to draw.", "elements");

            //The RandomNumericSequenceGenerator is only created for collections of minimum 2 elements
            if (this.elements.Length > 1)
                this.sequence = new RandomNumericSequenceGenerator(0, this.elements.Length - 1);
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

            return this.elements[this.GetNextIndex()];
        }

        private int GetNextIndex()
        {
            if (this.elements.Length == 1)
                return 0;
            else
                return (int)this.sequence.Create(typeof(int));
        }
    }
}