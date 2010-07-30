using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    internal class ComposerWithoutADefaultConstructor : DelegatingComposer
    {
        public ComposerWithoutADefaultConstructor(Func<ISpecimenBuilder> onCompose)
        {
            if (onCompose == null)
            {
                throw new ArgumentNullException("onCompose");
            }

            this.OnCompose = onCompose;
        }
    }
}
