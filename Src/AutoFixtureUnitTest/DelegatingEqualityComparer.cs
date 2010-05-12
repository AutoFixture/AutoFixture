namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.Collections;

    public class DelegatingEqualityComparer : IEqualityComparer
    {
        public DelegatingEqualityComparer()
        {
            this.OnEquals = (x, y) => false;
        }

        public bool Equals(object x, object y)
        {
            return this.OnEquals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }

        internal Func<object, object, bool> OnEquals { get; set; }
    }
}