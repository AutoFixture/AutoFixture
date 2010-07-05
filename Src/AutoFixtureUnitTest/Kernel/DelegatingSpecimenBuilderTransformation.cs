using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DelegatingSpecimenBuilderTransformation : ISpecimenBuilderTransformation
    {
        public DelegatingSpecimenBuilderTransformation()
        {
            this.OnTransform = b => b;
        }

        #region ISpecimenBuilderTransformation Members

        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            return this.OnTransform(builder);
        }

        #endregion

        internal Func<ISpecimenBuilder, ISpecimenBuilder> OnTransform { get; set; }
    }
}
