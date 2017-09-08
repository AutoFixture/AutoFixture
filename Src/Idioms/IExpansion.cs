using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an operation that somehow expands a single value into a sequence.
    /// </summary>
    /// <typeparam name="T">The type of item being expanded.</typeparam>
    public interface IExpansion<T>
    {
        /// <summary>
        /// Expands a value into a sequence of values.
        /// </summary>
        /// <param name="value">The value to expand.</param>
        /// <returns>A sequence of values.</returns>
        /// <remarks>
        /// <para>
        /// In a sense, an expansion is the opposite as an aggregation. Given a single value the
        /// result is a sequence produced from the value. There's no inherent definition on how
        /// such a sequence is produced or what the values in the sequence will be.
        /// <paramref name="value"/> may or may not be contained in the result.
        /// </para>
        /// <para>
        /// Implementations could, for example, simply produce a sequence with the input value as
        /// the single element, or it could produce a sequence where the input value is repeated a
        /// number of times. Other options are also possible.
        /// </para>
        /// </remarks>
        IEnumerable<T> Expand(T value);
    }
}
