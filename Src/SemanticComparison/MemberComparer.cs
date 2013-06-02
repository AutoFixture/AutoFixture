using System;
using System.Collections;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    public class MemberComparer : IMemberComparer
    {
        private readonly IEqualityComparer comparer;

        public MemberComparer(IEqualityComparer comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.comparer = comparer;
        }

        public IEqualityComparer Comparer
        {
            get { return this.comparer; }
        }

        public bool IsSatisfiedBy(PropertyInfo pi)
        {
            return true;
        }

        public bool IsSatisfiedBy(FieldInfo fi)
        {
            return true;
        }

        public new bool Equals(object x, object y)
        {
            return this.comparer.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}
