using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingMethodContext : IMethodContext
    {
        public DelegatingMethodContext()
        {
            this.OnVerifyBoundaries = convention => { };
        }

        public Action<IBoundaryConvention> OnVerifyBoundaries { get; set; }

        #region IMemberContext Members

        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            this.OnVerifyBoundaries(convention);
        }

        #endregion
    }
}
