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
    public class Seed : ICustomAttributeProvider, IEquatable<Seed>
    {
        private readonly Type targetType;
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seed"/> class.
        /// </summary>
        /// <param name="targetType">The type for which the seed applies.</param>
        /// <param name="value">The seed.</param>
        public Seed(Type targetType, object value)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
        
            this.targetType = targetType;
            this.value = value;
        }

        /// <summary>
        /// Gets the seed value.
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the type for which the seed applies.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
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
            var other = obj as Seed;
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
            return this.TargetType.GetHashCode() ^ (this.Value == null ? 0 : this.Value.GetHashCode());
        }

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

        /// <summary>
        /// Determines whether this instance equals another instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance equals <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(Seed other)
        {
            if (other == null)
            {
                return false;
            }
        
            return this.TargetType == other.TargetType
                && object.Equals(this.Value, other.Value);
        }
    }
}
