using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingSpecimenContainer : ISpecimenContainer
    {
        public DelegatingSpecimenContainer()
        {
            this.OnResolve = r => null;
        }

        #region ISpecimenContainer Members

        public object Resolve(object request)
        {
            return this.OnResolve(request);
        }

        #endregion

        internal Func<object, object> OnResolve { get; set; }
    }
}
