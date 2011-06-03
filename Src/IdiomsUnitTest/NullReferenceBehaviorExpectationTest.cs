using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class NullReferenceBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullReferenceBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TriState))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTime))]
        public void VerifyDoesNothingWhenContextTypeIsNotInterfaceOrReference(Type type)
        {
            // Fixture setup
            var verifyInvoked = false;
            var mockCommand = new DelegatingContextualCommand { OnExecute = v => verifyInvoked = true };
            mockCommand.ContextType = type;

            var sut = new NullReferenceBehaviorExpectation();
            // Exercise system
            sut.Verify(mockCommand);
            // Verify outcome
            Assert.False(verifyInvoked, "Mock verified.");
            // Teardown
        }
    }
}
