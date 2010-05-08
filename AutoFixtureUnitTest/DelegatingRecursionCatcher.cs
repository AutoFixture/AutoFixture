namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.Collections;
    using AutoFixture;
    using AutoFixture.Kernel;

    public class DelegatingRecursionCatcher : RecursionCatcher
    {
        public DelegatingRecursionCatcher(ISpecimenBuilder builder, IEqualityComparer comparer) : base(builder, comparer)
        {
        }

        public DelegatingRecursionCatcher(ISpecimenBuilder builder) : base(builder)
        {
        }

        public override object HandleRecursiveRequest(object request)
        {
            return this.OnHandleRecursiveRequest(request);
        }

        internal Func<object, object> OnHandleRecursiveRequest { get; set; }
    }
}