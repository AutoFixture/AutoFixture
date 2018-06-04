using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Generates a perpetual sequence of items.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// <para>
    /// This is a generator that generates a perpetual sequence of items of
    /// type <typeparamref name="T" />, based on an encapsulated
    /// <see cref="ISpecimenBuilder" />. This can be useful when zipping a
    /// against a finite sequence, since this sequence will go on for as long
    /// as required.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "A Generator is (or ought to be) a generally known concept, based on the Iterator design pattern.")]
    public class Generator<T> : IEnumerable<T>
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator{T}" />
        /// class.
        /// </summary>
        /// <param name="builder">
        /// A builder which is used to generate items.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="builder" /> is null.
        /// </exception>
        public Generator(ISpecimenBuilder builder)
        {
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            while (true)
                yield return this.builder.Create<T>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
