using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoFixture.Xunit3.Internal
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        ///     Applies a specified function to the corresponding elements of any number of sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="sequences">The input sequences.</param>
        /// <param name="resultSelector">A function that specifies how to combine the corresponding elements of the sequences.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains elements of the input sequences, combined by resultSelector.</returns>
        internal static IEnumerable<TResult> Zip<T, TResult>(this IEnumerable<IEnumerable<T>> sequences,
                                                             Func<IEnumerable<T>, TResult> resultSelector)
        {
            var enumerators = sequences.Select(s => s.GetEnumerator()).ToList();
            while (enumerators.TrueForAll(e => e.MoveNext()))
            {
                yield return resultSelector(enumerators.Select(e => e.Current));
            }
        }

        internal static IEnumerable<ITheoryDataRow> Zip(this IEnumerable<IEnumerable<ITheoryDataRow>> sequences,
                                                        Func<IEnumerable<ITheoryDataRow>, ITheoryDataRow> resultSelector)
        {
            var enumerators = sequences.Select(s => s.GetEnumerator()).ToList();
            while (enumerators.TrueForAll(e => e.MoveNext()))
            {
                yield return resultSelector(enumerators.Select(e => e.Current));
            }
        }

        /// <summary>
        ///     Collapses a series of sequences down by using items from the first sequence until it finishes,
        ///     then continuing from the same index through the second sequence, and so on until all sequences
        ///     have been exhausted.
        /// </summary>
        /// <param name="sequences">The input sequences.</param>
        /// <returns>Items from each sequence in turn, yielding those from the first sequence first.</returns>
        internal static IEnumerable<object> Collapse(this IEnumerable<ITheoryDataRow> sequences)
        {
            var position = 0;
            foreach (var sequence in sequences)
            {
                foreach (var item in sequence.GetData().Skip(position))
                {
                    position++;
                    yield return item;
                }
            }
        }
    }
}