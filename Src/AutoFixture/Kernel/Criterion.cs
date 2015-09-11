using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    public class Criterion<T> : IEquatable<T>
    {
        private readonly T target;
        private readonly IEqualityComparer<T> comparer;

        public Criterion(T target, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.target = target;
            this.comparer = comparer;
        }

        public bool Equals(T other)
        {
            return this.comparer.Equals(this.target, other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Criterion<T>;
            if (other == null)
                return base.Equals(obj);
            return object.Equals(this.target, other.target)
                && object.Equals(this.comparer, other.comparer);
        }

        public T Target
        {
            get { return this.target; }
        }

        public IEqualityComparer<T> Comparer
        {
            get { return this.comparer; }
        }

        public override int GetHashCode()
        {
            return this.target.GetHashCode() ^ this.comparer.GetHashCode();
        }
    }
}