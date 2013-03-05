using System;
using System.Collections;
using System.Collections.Generic;

namespace Ploeh.SemanticComparison
{
    /// <summary>An enumerable that simply returns the enumerator passed into the constructor.</summary>
    /// <typeparam name="T">The type of the objects returned by the enumerator</typeparam>
    public class EnumerableEnumerator<T> : IEnumerable<T>
    {
        private IEnumerator<T> enumerator;

        /// <summary>Initializes a new instance of the <see cref="EnumerableEnumerator{T}" /> class.</summary>
        /// <param name="enumerator">The enumerator.</param>
        public EnumerableEnumerator(IEnumerator<T> enumerator)
        {
            if (enumerator == null)
            {
                throw new ArgumentNullException("enumerator");
            }

            this.enumerator = enumerator;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="IEnumerator{T}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.enumerator;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}