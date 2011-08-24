using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Encapsulates a seed for a given type.
    /// </summary>
    /// <typeparam name="T">The type for which the seed applies.</typeparam>
    public class Seed<T> : ICustomAttributeProvider
    {
        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seed{T}"/> class.
        /// </summary>
        /// <param name="value">The seed.</param>
        public Seed(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the seed value.
        /// </summary>
        public T Value
        {
            get { return this.value; }
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
    }
}
