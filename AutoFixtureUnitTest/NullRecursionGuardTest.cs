using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    using Kernel;

    public class NullRecursionGuardTest
    {
        [Fact]
        public void SutIsRecursionGuard()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullRecursionGuard(new DelegatingSpecimenBuilder());
            // Verify outcome
            Assert.IsAssignableFrom<RecursionGuard>(sut);
            // Teardown
        }

        [Fact]
        public void ReturnsNullAtRecursionPoint()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new NullRecursionGuard(builder);
            var container = new DelegatingSpecimenContainer();
            container.OnResolve = (r) => sut.Create(r, container); // Provoke recursion

            // Exercise system
            object res = sut.Create(Guid.NewGuid(), container);

            Assert.Null(res);
        }
    }
}