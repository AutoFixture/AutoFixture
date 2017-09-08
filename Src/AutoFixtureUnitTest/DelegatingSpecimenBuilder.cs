using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class DelegatingSpecimenBuilder : ISpecimenBuilder
    {
        public DelegatingSpecimenBuilder()
        {
            this.OnCreate = (r, c) => null;
        }

        public object Create(object request, ISpecimenContainer container)
        {
            return this.OnCreate(request, container);
        }

        internal Func<object, ISpecimenContainer, object> OnCreate { get; set; }
    }
}
