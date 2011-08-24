using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
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
            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => sut.Create(r, container); // Provoke recursion

            // Exercise system
            object res = sut.Create(Guid.NewGuid(), container);

            Assert.Null(res);
        }
    }
}