using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingRequestSpecification : IRequestSpecification
    {
        public DelegatingRequestSpecification()
        {
            this.OnIsSatisfiedBy = r => false;
        }

        public bool IsSatisfiedBy(object request)
        {
            return this.OnIsSatisfiedBy(request);
        }

        internal Predicate<object> OnIsSatisfiedBy { get; set; }
    }
}
