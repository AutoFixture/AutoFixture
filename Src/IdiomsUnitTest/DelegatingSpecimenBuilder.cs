using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingSpecimenBuilder : ISpecimenBuilder
    {
        public DelegatingSpecimenBuilder()
        {
            this.OnCreate = (r, c) => new object();
        }

        public Func<object, ISpecimenContext, object> OnCreate { get; set; }

        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }

        #endregion
    }
}
