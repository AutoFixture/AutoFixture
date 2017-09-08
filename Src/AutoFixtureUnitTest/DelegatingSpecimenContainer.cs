using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class DelegatingSpecimenContainer : ISpecimenContainer
    {
        public DelegatingSpecimenContainer()
        {
            this.OnCreate = r => null;
        }

        public object Create(object request)
        {
            return this.OnCreate(request);
        }

        internal Func<object, object> OnCreate { get; set; }
    }
}
