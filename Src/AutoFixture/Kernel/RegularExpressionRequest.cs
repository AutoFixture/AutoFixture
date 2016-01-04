﻿using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a pattern for a regular expression.
    /// </summary>
    public class RegularExpressionRequest : IEquatable<RegularExpressionRequest>
    {
        private readonly string pattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularExpressionRequest"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public RegularExpressionRequest(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            this.pattern = pattern;
        }

        /// <summary>
        /// Gets the regular expression pattern.
        /// </summary>
        public string Pattern
        {
            get
            {
                return this.pattern;
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
            var other = obj as RegularExpressionRequest;
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
            return this.Pattern.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(RegularExpressionRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Pattern == other.Pattern;
        }
    }
}