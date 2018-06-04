using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Provides custom equality comparison for <see cref="MemberInfo"/> instances.
    /// </summary>
    public class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>, IEqualityComparer
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first <see cref="MemberInfo"/> instance to compare.</param>
        /// <param name="y">The second <see cref="MemberInfo"/> instance to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are considered equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="x"/> and <paramref name="y"/> are considered equal if x is exactly
        /// equal to y. If not, they are still considered equal if both instances'
        /// <see cref="MemberInfo.DeclaringType"/> and <see cref="MemberInfo.Name"/> properties are
        /// equal.
        /// </para>
        /// </remarks>
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null)
            {
                return false;
            }
            if (y == null)
            {
                return false;
            }

            if (x.Equals(y))
            {
                return true;
            }

            if (x.DeclaringType == null)
            {
                return false;
            }
            if (y.DeclaringType == null)
            {
                return false;
            }

            return AreTypesRelated(x.DeclaringType, y.DeclaringType)
                && string.Equals(x.Name, y.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for a <see cref="MemberInfo"/> instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for the supplied instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is
        /// null.
        /// </exception>
        public int GetHashCode(MemberInfo obj)
        {
            if (obj == null) return 0;

            if (obj.DeclaringType == null)
            {
                return obj.Name.GetHashCode();
            }
            return obj.DeclaringType.GetHashCode() ^ obj.Name.GetHashCode();
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            if ((x == null) && (y == null))
            {
                return true;
            }

            var miX = x as MemberInfo;
            if (miX == null)
            {
                return false;
            }

            var miY = y as MemberInfo;
            if (miY == null)
            {
                return false;
            }

            return this.Equals(miX, miY);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var mi = obj as MemberInfo;
            if (mi != null)
            {
                return this.GetHashCode(mi);
            }

            return obj.GetHashCode();
        }

        private static bool AreTypesRelated(Type x, Type y)
        {
            return x.GetTypeInfo().IsAssignableFrom(y)
                || y.GetTypeInfo().IsAssignableFrom(x);
        }
    }
}
