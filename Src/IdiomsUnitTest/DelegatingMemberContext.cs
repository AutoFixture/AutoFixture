using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingMemberContext : IMemberContext
    {
        public DelegatingMemberContext()
        {
            this.OnVerifyBoundaries = c => { };
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
