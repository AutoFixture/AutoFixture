using System;
using System.Reflection;

namespace Ploeh.SemanticComparison.UnitTest
{
    internal class DelegatingMemberComparer : IMemberComparer
    {
        internal DelegatingMemberComparer()
        {
            this.OnIsSatisfiedByProperty = x => false;
            this.OnIsSatisfiedByField = x => false;
            this.OnEquals = (x, y) => false;
        }

        public bool IsSatisfiedBy(PropertyInfo pi)
        {
            return this.OnIsSatisfiedByProperty(pi);
        }

        public bool IsSatisfiedBy(FieldInfo fi)
        {
            return this.OnIsSatisfiedByField(fi);
        }

        public new bool Equals(object x, object y)
        {
            return this.OnEquals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }

        internal Func<PropertyInfo, bool> OnIsSatisfiedByProperty { get; set; }
        internal Func<FieldInfo, bool> OnIsSatisfiedByField { get; set; }
        internal Func<object, object, bool> OnEquals { get; set; }
    }
}
