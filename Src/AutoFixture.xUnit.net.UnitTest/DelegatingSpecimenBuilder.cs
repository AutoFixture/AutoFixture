using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    internal class DelegatingSpecimenBuilder : ISpecimenBuilder
    {
        public DelegatingSpecimenBuilder()
        {
            this.OnCreate = (r, c) => new object();
        }

        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }

        #endregion

        internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
    }
}
