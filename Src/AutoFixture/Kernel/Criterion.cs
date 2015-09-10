using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    public class Criterion<T> : IEquatable<T>
    {
        public Criterion(T target, IEqualityComparer<T> comparer)
        {
        }

        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }
    }
}