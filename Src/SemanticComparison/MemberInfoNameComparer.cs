using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Compares <see cref="MemberInfo"/> instances.
    /// </summary>
    public class MemberInfoNameComparer : IEqualityComparer<MemberInfo>
    {
        /// <summary>
        /// Compares two <see cref="MemberInfo"/> instances for equality.
        /// </summary>
        /// <param name="x">The first instance to compare.</param>
        /// <param name="y">The second instance to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> has the same
        /// name; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            return ((x == y) || (((x != null) && (y != null)) && object.Equals(x.Name, y.Name)));
        }

        /// <summary>
        /// Returns the hash code of the <see cref="MemberInfo"/> instance's
        /// <see cref="MemberInfo.Name"/>.
        /// </summary>
        /// <param name="obj">The <see cref="MemberInfo"/> for which to return a hash code.</param>
        /// <returns>
        /// The hash code of the <see cref="MemberInfo"/> instance's <see cref="MemberInfo.Name"/>.
        /// </returns>
        public int GetHashCode(MemberInfo obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            return obj.Name.GetHashCode();
        }
    }
}
