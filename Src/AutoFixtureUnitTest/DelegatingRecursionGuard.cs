namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.Collections;
    using AutoFixture;
    using AutoFixture.Kernel;

    public class DelegatingRecursionGuard : RecursionGuard
    {
        public DelegatingRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer) : base(builder, comparer)
        {
        }

        public DelegatingRecursionGuard(ISpecimenBuilder builder) : base(builder)
        {
        }

        public override object HandleRecursiveRequest(object request)
        {
            return this.OnHandleRecursiveRequest(request);
        }

        internal Func<object, object> OnHandleRecursiveRequest { get; set; }
    }
}