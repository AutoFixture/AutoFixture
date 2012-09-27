using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents a sequence of limits. Limits consist of a sequence of integers beginning
    /// with two positive or negative number optionally followed by a series of greater numbers.
    /// </summary>
    /// <remarks>
    /// The default upper limits are 255, 32767, and 2147483647.
    /// </remarks>
    public class RandomNumericSequenceLimit : IEquatable<RandomNumericSequenceLimit>
    {
        private readonly int[] limit;

        private int currentLower;
        private int currentUpper;
        private int currentCount;

        internal event EventHandler CurrentRangeExceeded;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceLimit" /> class
        /// with the default limits, 255, 32767, and 2147483647.
        /// </summary>
        public RandomNumericSequenceLimit()
            : this(1, Byte.MaxValue, Int16.MaxValue, Int32.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceLimit" /> class.
        /// </summary>
        /// <param name="limit">A sequence of integers beginning with two positive or negative 
        /// number optionally followed by a series of greater numbers.</param>
        public RandomNumericSequenceLimit(IEnumerable<int> limit)
            : this(limit.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceLimit" /> class.
        /// </summary>
        /// <param name="limit">An array of integers beginning with two positive or negative 
        /// number optionally followed by a series of greater numbers.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public RandomNumericSequenceLimit(params int[] limit)
        {
            if (limit == null)
            {
                throw new ArgumentNullException("limit");
            }

            if (limit.Length < 2)
            {
                throw new ArgumentException("The limit must be a sequence of two or more integers.");
            }

            this.limit = limit;
            this.SetInitialRange();
        }

        /// <summary>
        /// Gets the sequence of limits.
        /// </summary>
        /// <value>
        /// The sequence of limits.
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
                && this.currentLower == other.currentLower
                && this.currentUpper == other.currentUpper
                && this.currentCount == other.currentCount;
        }

        internal int CurrentLower
        {
            get { return this.currentLower; }
        }

        internal int CurrentUpper
        {
            get { return this.currentUpper; }
        }

        internal virtual void OnCurrentRangeExceeded()
        {
            EventHandler handler = this.CurrentRangeExceeded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        internal void Evaluate()
        {
            if (this.currentCount == (this.currentUpper - this.currentLower))
            {
                this.currentCount = 0;

                try
                {
                    this.SetNextRange();
                }
                catch(InvalidOperationException)
                {
                    this.SetInitialRange();
                }

                this.OnCurrentRangeExceeded();
            }

            this.currentCount++;
        }

        private void SetInitialRange()
        {
            this.currentLower = limit[0];
            this.currentUpper = limit[1];
        }

        private void SetNextRange()
        {
            var remaining = this.limit.Where(x => x > this.currentUpper - 1).ToArray();
            if (remaining.Any())
            {
                this.currentLower = this.currentUpper;
                this.currentUpper = remaining.Min() + 1;
            }
            else
            {
                throw new InvalidOperationException("The upper limit has been reached.");
            }
        }
    }
}
