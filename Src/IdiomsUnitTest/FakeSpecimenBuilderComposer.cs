using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class FakeSpecimenBuilderComposer : ISpecimenBuilderComposer
    {
        private readonly IDictionary<object,object> specimens;

        public FakeSpecimenBuilderComposer()
        {
            this.specimens = new Dictionary<object, object>();
        }

        public IDictionary<object, object> Specimens
        {
            get { return this.specimens; }
        }

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    return this.Specimens[r];
                }
            };
        }

        #endregion
    }
}
