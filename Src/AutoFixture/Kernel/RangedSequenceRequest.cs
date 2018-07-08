using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Request for the sequence of <see cref="Request"/> values.
    /// Sequece size is constrained by the <see cref="MinLength"/> and <see cref="MaxLength"/> values.
    /// </summary>
    public class RangedSequenceRequest : IEquatable<RangedSequenceRequest>
    {
        /// <summary>
        /// Gets the request the sequence should contain result of.
        /// </summary>
        public object Request { get; }

        /// <summary>
        /// Gets the minimum number of items in the sequence.
        /// </summary>
        public int MinLength { get; }

        /// <summary>
        /// Gets the maximum number of items in the sequence.
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RangedSequenceRequest"/>.
        /// </summary>
        public RangedSequenceRequest(object request, int minLength, int maxLength)
        {
            if (minLength < 0)
                throw new ArgumentOutOfRangeException(nameof(minLength), minLength, "Min length cannot be less than zero.");
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Max length cannot be less than zero.");
            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Max length cannot be less than min length.");

            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.MinLength = minLength;
            this.MaxLength = maxLength;
        }

        /// <inheritdoc />
        public bool Equals(RangedSequenceRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.Request.Equals(other.Request)
                   && this.MinLength == other.MinLength
                   && this.MaxLength == other.MaxLength;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is RangedSequenceRequest sequenceRequest && this.Equals(sequenceRequest);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Request.GetHashCode();
                hashCode = (hashCode * 397) ^ this.MinLength;
                hashCode = (hashCode * 397) ^ this.MaxLength;
                return hashCode;
            }
        }
    }
}