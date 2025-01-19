using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Xunit3.UnitTest
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Applies a specified function to the corresponding elements of any number of sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="sequences">The input sequences.</param>
        /// <param name="resultSelector">A function that specifies how to combine the corresponding elements of the sequences.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements of the input sequences, combined by resultSelector.</returns>
        internal static IEnumerable<TResult> Zip<T, TResult>(this IEnumerable<IEnumerable<T>> sequences,
            Func<IEnumerable<T>, TResult> resultSelector)
        {
            var enumerators = sequences.Select(s => s.GetEnumerator()).ToList();
            while (enumerators.TrueForAll(e => e.MoveNext()))
            {
                yield return resultSelector(enumerators.Select(e => e.Current));
            }
        }

        /// <summary>
        /// Collapses a series of sequences down by using items from the first sequence until it finishes,
        /// then continuing from the same index through the second sequence, and so on until all sequences
        /// have been exhausted.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="sequences">The input sequences.</param>
        /// <returns>Items from each sequence in turn, yielding those from the first sequence first.</returns>
        internal static IEnumerable<T> Collapse<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            var position = 0;
            foreach (var sequence in sequences)
            {
                foreach (var item in sequence.Skip(position))
                {
                    position++;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Casts the source sequence to an <see cref="IReadOnlyCollection{T}"/>.
        /// Enumerates the source sequence to an array if it is not an enumerated collection.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <typeparam name="T">The sequence item type.</typeparam>
        /// <returns> An <see cref="IReadOnlyCollection{T}"/> that contains elements of the source sequence.</returns>
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return source is not null
                ? source as IReadOnlyCollection<T> ?? source.ToArray()
                : null;
        }
    }
}