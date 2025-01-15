using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoFixture
{
    /// <summary>
    /// A collection of enum values.
    /// </summary>
    /// <typeparam name="TEnum">The type of enum values.</typeparam>
    public sealed class EnumValueCollection<TEnum> : IEnumerable<TEnum>
        where TEnum : struct, Enum
    {
        private readonly IEnumerable<TEnum> values;

        internal EnumValueCollection(IEnumerable<TEnum> values)
        {
            this.values = values;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of enum values.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection of enum values.</returns>
        public IEnumerator<TEnum> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
