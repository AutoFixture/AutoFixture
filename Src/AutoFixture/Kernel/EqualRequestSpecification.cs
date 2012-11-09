using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class EqualRequestSpecification : IRequestSpecification
    {
        private readonly object target;
        private readonly IEqualityComparer comparer;

        public EqualRequestSpecification(object target)
            : this(target, EqualityComparer<object>.Default)
        {
        }

        public EqualRequestSpecification(
            object target,
            IEqualityComparer comparer)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.target = target;
            this.comparer = comparer;
        }

        public bool IsSatisfiedBy(object request)
        {
            return this.comparer.Equals(this.target, request);
        }

        public object Target
        {
            get { return this.target; }
        }
    }
}
