using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
	public class ThrowingRecursionGuardTest
	{
		[Fact]
		public void SutIsRecursionGuard()
		{
			// Fixture setup
			// Exercise system
			var sut = new ThrowingRecursionGuard(new DelegatingSpecimenBuilder());
			// Verify outcome
			Assert.IsAssignableFrom<RecursionGuard>(sut);
			// Teardown
		}

		[Fact]
		public void ThrowsAtRecursionPoint()
		{
			// Fixture setup
			var builder = new DelegatingSpecimenBuilder();
			builder.OnCreate = (r, c) => c.Resolve(r);
			var sut = new ThrowingRecursionGuard(builder);
			var container = new DelegatingSpecimenContext();
			container.OnResolve = (r) => sut.Create(r, container); // Provoke recursion

			// Exercise system
			Assert.Throws(typeof(ObjectCreationException), () => sut.Create(Guid.NewGuid(), container));
		}
	}
}