using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Signals that many similar instances are requested.
    /// </summary>
    public class ManyRequest
    {
        private readonly object request;
        private readonly int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyRequest"/> class.
        /// </summary>
        /// <param name="request">The underlying request to multiply.</param>
        public ManyRequest(object request)
            : this(request, 0)
        {
        }

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

            this.request = request;
            this.count = count;
        }

        /// <summary>
        /// Creates many requests from the underlying requests.
        /// </summary>
        /// <param name="defaultCount">
        /// A default count that can be used if no specific number of instances is requested.
        /// </param>
        /// <returns>A number of similar requests.</returns>
        /// <remarks>
        /// <para>
        /// The number of requests returned depends on whether a number was specified along with
        /// the request. If a positive number was specified, this is used; otherwise,
        /// <paramref name="defaultCount"/> is used.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When both the requested count and <paramref name="defaultCount"/> are negative or zero.
        /// </exception>
        public IEnumerable<object> CreateRequests(int defaultCount)
        {
            var i = this.GetCount(defaultCount);
            return Enumerable.Repeat(this.request, i);
        }

        private int GetCount(int defaultCount)
        {
            if (this.count > 0)
            {
                return this.count;
            }

            if (defaultCount > 0)
            {
                return defaultCount;
            }

            throw new ArgumentOutOfRangeException("defaultNumber", "Neither the contained count nor the default count is positive. At least one of them must specify a positive number of requests to generate.");
        }
    }
}
