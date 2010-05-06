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
            this.OnCreate = r => null;
        }

        #region ISpecimenContainer Members

        public object Resolve(object request)
        {
            return this.OnCreate(request);
        }

        #endregion

        internal Func<object, object> OnCreate { get; set; }
    }
}
