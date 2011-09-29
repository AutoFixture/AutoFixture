using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a maximum length for a string.
    /// </summary>
    public class ConstrainedStringRequest : IEquatable<ConstrainedStringRequest>
    {
        private readonly int maximumLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedStringRequest"/> class.
        /// </summary>
        /// <param name="maximumLength">The maximum.</param>
        public ConstrainedStringRequest(int maximumLength)
        {
            if (maximumLength < 0)
            {
                throw new ArgumentOutOfRangeException("maximumLength", "Maximum length must be equal or greater than 0.");
            }

            this.maximumLength = maximumLength;
        }

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        public int MaximumLength
        {
            get
            {
                return this.maximumLength;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            var other = obj as ConstrainedStringRequest;
            if (other != null)
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
            return this.MaximumLength.GetHashCode();
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

            return this.MaximumLength == other.MaximumLength;
        }
    }
}