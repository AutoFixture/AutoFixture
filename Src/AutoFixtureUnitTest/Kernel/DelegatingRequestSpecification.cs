using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingRequestSpecification : IRequestSpecification
    {
        public DelegatingRequestSpecification()
        {
            this.OnIsSatisfiedBy = r => false;
        }

        #region IRequestSpecification Members

        public bool IsSatisfiedBy(object request)
        {
            return this.OnIsSatisfiedBy(request);
        }

        #endregion

        internal Predicate<object> OnIsSatisfiedBy { get; set; }
    }
}
