using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a maximum length for a string.
    /// </summary>
    public class ConstrainedStringRequest : IEquatable<ConstrainedStringRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedStringRequest"/> class.
        /// </summary>
        /// <param name="minimumLength">The minimum length.</param>
        /// <param name="maximumLength">The maximum length.</param>
        public ConstrainedStringRequest(int minimumLength, int maximumLength)
        {
            if (minimumLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength), "Minimum length must be equal or greater than 0.");
            }

            if (maximumLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumLength), "Maximum length must be equal or greater than 0.");
            }

            if (maximumLength < minimumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumLength), "Maximum length must be equal or greater than Minimum length.");
            }

            this.MinimumLength = minimumLength;
            this.MaximumLength = maximumLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedStringRequest"/> class.
        /// </summary>
        /// <param name="maximumLength">The maximum.</param>
        public ConstrainedStringRequest(int maximumLength)
            : this(0, maximumLength)
        {
        }

        /// <summary>
        /// Gets the minimum length.
        /// </summary>
        public int MinimumLength { get; }

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        public int MaximumLength { get; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj is ConstrainedStringRequest other)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.MinimumLength.GetHashCode()
                 ^ this.MaximumLength.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ConstrainedStringRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.MinimumLength == other.MinimumLength &&
                   this.MaximumLength == other.MaximumLength;
        }
    }
}