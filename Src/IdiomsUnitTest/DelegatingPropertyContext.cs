using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingPropertyContext : IPropertyContext
    {
        public DelegatingPropertyContext()
        {
            this.OnVerifyWritable = () => { };
            this.OnVerifyBoundaries = convention => { };
        }

        public Action OnVerifyWritable { get; set; }

        public Action<IBoundaryConvention> OnVerifyBoundaries { get; set; }

        #region IPropertyContext Members

        public void VerifyWritable()
        {
            this.OnVerifyWritable();
        }

        #endregion

        #region IMemberContext Members

        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            this.OnVerifyBoundaries(convention);
        }

        #endregion
    }
}
