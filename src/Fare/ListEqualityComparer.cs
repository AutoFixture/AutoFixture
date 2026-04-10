using System;
using System.Collections.Generic;
using System.Linq;

namespace Fare
{
    internal sealed class ListEqualityComparer<T>  : IEqualityComparer<List<T>>, IEquatable<ListEqualityComparer<T>>
    {
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ListEqualityComparer<T> left, ListEqualityComparer<T> right)
        {
            return object.Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ListEqualityComparer<T> left, ListEqualityComparer<T> right)
        {
            return !object.Equals(left, right);
        }

        /// <inheritdoc />
        public bool Equals(List<T> x, List<T> y)
        {
            if (x.Count != y.Count)
            {
                return false;
            }

            return x.SequenceEqual(y);
        }

        /// <inheritdoc />
        public int GetHashCode(List<T> obj)
        {
            // http://stackoverflow.com/questions/1079192/is-it-possible-to-combine-hash-codes-for-private-members-to-generate-a-new-hash
            return obj.Aggregate(17, (current, item) => (current * 31) + item.GetHashCode());
        }

        /// <inheritdoc />
        public bool Equals(ListEqualityComparer<T> other)
        {
            return !object.ReferenceEquals(null, other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(ListEqualityComparer<T>))
            {
                return false;
            }

            return this.Equals((ListEqualityComparer<T>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
