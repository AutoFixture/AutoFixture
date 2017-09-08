using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingSpecimenContext : ISpecimenContext
    {
        public DelegatingSpecimenContext()
        {
            this.OnResolve = r => null;
        }

        public object Resolve(object request)
        {
            return this.OnResolve(request);
        }

        internal Func<object, object> OnResolve { get; set; }
    }
}
