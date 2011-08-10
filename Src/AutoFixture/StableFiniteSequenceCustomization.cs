using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class StableFiniteSequenceCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.Customizations.Add(new StableFiniteSequenceRelay());
        }

        #endregion
    }
}
