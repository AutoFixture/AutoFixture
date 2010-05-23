using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Signals that many similar instances are requested.
    /// </summary>
    public class ManyRequest : IEquatable<ManyRequest>
    {
        private readonly object request;
        private readonly int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyRequest"/> class.
        /// </summary>
        /// <param name="request">The underlying request to muliply.</param>
        /// <param name="count">The number of instances requested.</param>
        public ManyRequest(object request, int count)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", string.Format(CultureInfo.CurrentCulture, "The requested count must be a positive number, but was {0}.", count));
            }        

            this.request = request;
            this.count = count;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Object"/> is equal to this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            var other = obj as ManyRequest;
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// When both the requested count and <paramref name="defaultCount"/> are negative or zero.
        /// </exception>
        public IEnumerable<object> CreateRequests()
        {
            return Enumerable.Repeat(this.request, this.count);
        }

        #region IEquatable<ManyRequest> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> represents the same request with the
        /// same requested count; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(ManyRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.request.Equals(other.request)
                && this.count == other.count;
        }

        #endregion
    }
}
