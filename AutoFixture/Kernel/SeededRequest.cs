using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a seed for a given type.
    /// </summary>
    public class SeededRequest : ICustomAttributeProvider, IEquatable<SeededRequest>
    {
        private readonly object request;
        private readonly object seed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seed"/> class.
        /// </summary>
        /// <param name="request">The request for which the seed applies.</param>
        /// <param name="seed">The seed.</param>
        public SeededRequest(object request, object seed)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
        
            this.request = request;
            this.seed = seed;
        }

        /// <summary>
        /// Gets the seed value.
        /// </summary>
        public object Seed
        {
            get { return this.seed; }
        }

        /// <summary>
        /// Gets the inner request for which the seed applies.
        /// </summary>
        public object Request
        {
            get { return this.request; }
        }

        /// <summary>
        /// Determines whether this instance equals another instance.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance equals <paramref name="obj"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as SeededRequest;
            if (other != null)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for the instance.
        /// </summary>
        /// <returns>The hash code for the instance.</returns>
        public override int GetHashCode()
        {
            return this.Request.GetHashCode() ^ (this.Seed == null ? 0 : this.Seed.GetHashCode());
        }

        #region ICustomAttributeProvider Members

        /// <summary>
        /// Returns an array of all of the custom attributes defined on this member, excluding
        /// named attributes, or an empty array if there are no custom attributes. 
        /// </summary>
        /// <param name="inherit">
        /// When <see langword="true"/>, look up the hierarchy chain for the inherited custom
        /// attribute.
        /// </param>
        /// <returns>
        /// An array of Objects representing custom attributes, or an empty array.
        /// </returns>
        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        /// <summary>
        /// Returns an array of all of the custom attributes defined on this member, excluding
        /// named attributes, or an empty array if there are no custom attributes. 
        /// </summary>
        /// <param name="attributeType">The type of the custom attributes.</param>
        /// <param name="inherit">
        /// When <see langword="true"/>, look up the hierarchy chain for the inherited custom
        /// attribute.
        /// </param>
        /// <returns>
        /// An array of Objects representing custom attributes, or an empty array.
        /// </returns>
        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[0];
        }

        /// <summary>
        /// Indicates whether one or more instance of attributeType is defined on this member.
        /// </summary>
        /// <param name="attributeType">The type of the custom attributes.</param>
        /// <param name="inherit">
        /// When <see langword="true"/>, look up the hierarchy chain for the inherited custom
        /// attribute.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the attributeType is defined on this member;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        #endregion

        #region IEquatable<Seed> Members

        /// <summary>
        /// Determines whether this instance equals another instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance equals <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(SeededRequest other)
        {
            if (other == null)
            {
                return false;
            }
        
            return this.Request == other.Request
                && object.Equals(this.Seed, other.Seed);
        }

        #endregion
    }
}
