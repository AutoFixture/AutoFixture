using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingVerifiableBoundary : IVerifiableBoundary
    {
        public DelegatingVerifiableBoundary()
        {
            this.OnVerifyBoundaryBehavior = c => { };
        }

        public Action<IBoundaryConvention> OnVerifyBoundaryBehavior { get; set; }

        #region IVerifiableBoundary Members

        public void VerifyBoundaryBehavior(IBoundaryConvention convention)
        {
            this.OnVerifyBoundaryBehavior(convention);
        }

        #endregion
    }
}
