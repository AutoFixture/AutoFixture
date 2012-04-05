using System;
using System.Collections;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

namespace Ploeh.AutoFixtureUnitTest
{
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

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new DelegatingRecursionGuard(new CompositeSpecimenBuilder(builders));
        }

        internal Func<object, object> OnHandleRecursiveRequest { get; set; }
    }
}