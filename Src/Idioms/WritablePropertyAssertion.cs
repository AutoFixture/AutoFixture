using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class WritablePropertyAssertion : IdiomaticAssertion
    {
        private ISpecimenBuilderComposer composer;

        public WritablePropertyAssertion(ISpecimenBuilderComposer composer)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            this.composer = composer;
        }

        public Kernel.ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }
    }
}
