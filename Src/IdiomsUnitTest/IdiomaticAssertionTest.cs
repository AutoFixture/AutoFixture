using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class IdiomaticAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(IIdiomaticAssertion).IsAssignableFrom(typeof(IdiomaticAssertion)));
            // Teardown
        }

        [Fact]
        public void TestableSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingIdiomaticAssertion();
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyAssemblyCorrectlyInvokesNextVerify()
        {
            // Fixture setup
            var assembly = this.GetType().Assembly;
            var expectedTypes = assembly.GetExportedTypes();

            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnTypeArrayVerify = t => mockVerified = expectedTypes.SequenceEqual(t) };
            // Exercise system
            sut.Verify(assembly);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }
    }
}
