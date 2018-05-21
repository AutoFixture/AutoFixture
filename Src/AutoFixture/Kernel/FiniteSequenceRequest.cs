using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Signals that many similar instances are requested.
    /// </summary>
    public class FiniteSequenceRequest : IEquatable<FiniteSequenceRequest>
    {
        private readonly object request;
        private readonly int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteSequenceRequest"/> class.
        /// </summary>
        /// <param name="request">The underlying request to multiply.</param>
        /// <param name="count">The number of instances requested.</param>
        public FiniteSequenceRequest(object request, int count)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), string.Format(CultureInfo.CurrentCulture, "The requested count must be a positive number (or zero), but was {0}.", count));

            this.request = request;
            this.count = count;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is FiniteSequenceRequest other)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
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
            return this.request.GetHashCode() ^ this.count.GetHashCode();
        }

        /// <summary>
        /// Creates many requests from the underlying requests.
        /// </summary>
        /// <returns>A number of similar requests.</returns>
        public IEnumerable<object> CreateRequests()
        {
            return Enumerable.Repeat(this.request, this.count);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> represents the same request with the
        /// same requested count; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(FiniteSequenceRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.request.Equals(other.request)
                && this.count == other.count;
        }
    }
}
