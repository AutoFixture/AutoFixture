using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    public class Criterion<T> : IEquatable<T>
    {
        public Criterion(T target, IEqualityComparer<T> comparer)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (comparer == null)
                throw new ArgumentNullException("comparer");
        }

        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }
    }
}