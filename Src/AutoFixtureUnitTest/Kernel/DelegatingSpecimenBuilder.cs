using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingSpecimenBuilder : ISpecimenBuilder
    {
        public DelegatingSpecimenBuilder()
        {
            this.OnCreate = (r, c) => null;
        }

        public object Create(object request, ISpecimenContext container)
        {
            return this.OnCreate(request, container);
        }

        internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
    }
}
