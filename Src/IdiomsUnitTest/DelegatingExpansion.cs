using System;
using System.Collections.Generic;
using AutoFixture.Idioms;

namespace AutoFixture.IdiomsUnitTest
{
    public class DelegatingExpansion<T> : IExpansion<T>
    {
        public DelegatingExpansion()
        {
            this.OnExpand = v => new[] { v };
        }

        public Func<T, IEnumerable<T>> OnExpand { get; set; }

        public IEnumerable<T> Expand(T value)
        {
            return this.OnExpand(value);
        }
    }
}
