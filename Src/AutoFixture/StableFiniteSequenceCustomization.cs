using System;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that makes sequences (i.e. <see cref="IEnumerable{T}" /> stable instead of
    /// dynamic.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A sequence (i.e. an instance of <see cref="IEnumerable{T}" /> is not guaranteed to be
    /// stable or finite; it might, in fact, represent a Generator that creates an Iterator that
    /// can go on for ever. Even if the sequence is finite, it may not be stable because it may
    /// yield new items every time it's iterated.
    /// </para>
    /// <para>
    /// The StableFiniteSequenceCustomization class encapsulates a change in behavior that
    /// guarantees that an <see cref="IFixture" /> instance will resolve a request for a sequence
    /// into a stable sequence.
    /// </para>
    /// </remarks>
    /// <seealso cref="StableFiniteSequenceRelay" />
    [Obsolete("The StableFiniteSequenceRelay is now used by default, therefore this customization " +
              "is no longer needed and will be removed in future versions of AutoFixture.")]
    public class StableFiniteSequenceCustomization : ICustomization
    {
        /// <summary>
        /// Adds a <see cref="StableFiniteSequenceRelay" /> to an <see cref="IFixture" />.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <seealso cref="StableFiniteSequenceCustomization" />
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(new StableFiniteSequenceRelay());
        }
    }
}
