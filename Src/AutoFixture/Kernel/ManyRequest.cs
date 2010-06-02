using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Represents a request for many (an unspecified number) of specimens.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The difference between <see cref="ManyRequest"/> and <see cref="FiniteSequenceRequest"/> is
    /// that the latter specifies the number of specimens requested.
    /// </para>
    /// <para>
    /// <see cref="ManyTranslator"/> translates <see cref="ManyRequest"/> instances to
    /// <see cref="FiniteSequenceRequest"/> instances.
    /// </para>
    /// </remarks>
    /// <seealso cref="FiniteSequenceRequest"/>
    /// <seealso cref="ManyTranslator"/>
    public class ManyRequest
    {
        private readonly object request;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyRequest"/> class.
        /// </summary>
        /// <param name="request">A single request which will be multiplied.</param>
        public ManyRequest(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            this.request = request;
        }

        /// <summary>
        /// Gets the request to multiply.
        /// </summary>
        public object Request
        {
            get { return this.request; }
        }
    }
}
