﻿using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Represents a request for many (an unspecified number) of specimens.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The difference between <see cref="MultipleRequest"/> and
    /// <see cref="FiniteSequenceRequest"/> is that the latter specifies the number of specimens
    /// requested.
    /// </para>
    /// <para>
    /// <see cref="MultipleRelay"/> translates <see cref="MultipleRequest"/> instances to
    /// <see cref="FiniteSequenceRequest"/> instances.
    /// </para>
    /// </remarks>
    /// <seealso cref="FiniteSequenceRequest"/>
    /// <seealso cref="MultipleRelay"/>
    public class MultipleRequest : IEquatable<MultipleRequest>
    {
        private readonly object request;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleRequest"/> class.
        /// </summary>
        /// <param name="request">A single request which will be multiplied.</param>
        public MultipleRequest(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="MultipleRequest"/>
        /// instance which is equal to this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as MultipleRequest;
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
            return this.Request.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current <see cref="MultipleRequest"/> is equal to another
        /// MultipleRequest instance.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current <see cref="MultipleRequest"/> is equal to the
        /// <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MultipleRequest other)
        {
            if (other == null)
            {
                return false;
            }
        
            return object.Equals(this.Request, other.Request);
        }
    }
}
