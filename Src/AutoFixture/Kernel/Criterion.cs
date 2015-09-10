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
            if (target == null)
                throw new ArgumentNullException("target");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.target = target;
            this.comparer = comparer;
        }

        public bool Equals(T other)
        {
            return this.comparer.Equals(this.target, other);
        }
    }
}