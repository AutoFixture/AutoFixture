using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents a sequence of upper limits. Upper limits consist of a sequence of integers
    /// beginning with any positive number optionally followed by a series of greater numbers.
    /// </summary>
    /// <remarks>
    /// The default upper limits are 255, 32767, and 2147483647.
    /// </remarks>
    public class RandomNumericSequenceLimit : IEquatable<RandomNumericSequenceLimit>
    {
        private readonly IEnumerable<int> limit;

        private int lower;
        private int upper;
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceLimit" /> class
        /// with the default upper limits, 255, 32767, and 2147483647.
        /// </summary>
        public RandomNumericSequenceLimit()
            : this(Byte.MaxValue, Int16.MaxValue, Int32.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceLimit" /> class.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public RandomNumericSequenceLimit(IEnumerable<int> limit)
            : this(limit.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceLimit" /> class.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public RandomNumericSequenceLimit(params int[] limit)
        {
            if (limit == null)
            {
                throw new ArgumentNullException("limit");
            }

            if (limit.Any(x => x <= 1))
            {
                throw new ArgumentException(
                    "The value of the upper limits must be greater than 1.");
            }

            this.limit = limit;
            this.upper = 1;
            this.Evaluate();
        }

        /// <summary>
        /// Gets the sequence of upper limits.
        /// </summary>
        /// <value>
        /// The sequence of upper limits.
        /// </value>
        public IEnumerable<int> Limit
        {
            get { return this.limit; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as RandomNumericSequenceLimit;
            if (other == null)
            {
                return false;
            }

            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.limit.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter;
        /// otherwise, false.
        /// </returns>
        public bool Equals(RandomNumericSequenceLimit other)
        {
            if (other == null)
            {
                return false;
            }
            
            return this.limit.SequenceEqual(other.limit)
                && this.lower == other.lower
                && this.upper == other.upper
                && this.count == other.count;
        }

        internal int Lower
        {
            get { return this.lower; }
        }

        internal int Upper
        {
            get { return this.upper; }
        }

        internal void Evaluate()
        {
            if (this.count == (this.upper - this.lower))
            {
                this.MoveToNextRange();
            }

            this.count++;
        }

        private void MoveToNextRange()
        {
            this.lower = this.upper;
            this.upper = this.GetNextUpperLimit();
            this.count = 0;
        }

        private int GetNextUpperLimit()
        {
            int[] remaining = limit.Where(x => x > this.upper - 1).ToArray();
            if (remaining.Any())
            {
                return remaining.Min() + 1;
            }

            throw new ArgumentException("The upper limit has been reached.");
        }
    }
}
