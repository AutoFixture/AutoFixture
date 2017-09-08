using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructedWithArrayBehaviorExpectationsIsCorrect()
        {
            // Fixture setup
            var expectations = new[] { new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation() };
            var sut = new CompositeBehaviorExpectation(expectations);
            // Exercise system
            IEnumerable<IBehaviorExpectation> result = sut.BehaviorExpectations;
            // Verify outcome
            Assert.True(expectations.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ConstructedWithEnumerableBehaviorExpectationsIsCorrect()
        {
            // Fixture setup
            var expectations = new[] { new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation() }.Cast<IBehaviorExpectation>();
            var sut = new CompositeBehaviorExpectation(expectations);
            // Exercise system
            var result = sut.BehaviorExpectations;
            // Verify outcome
            Assert.True(expectations.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void VerifyVerifiesAllBehaviorExpectations()
        {
            // Fixture setup
            var observedCommands = new List<IGuardClauseCommand>();
            var expectations = Enumerable.Repeat(new DelegatingBehaviorExpectation { OnVerify = observedCommands.Add }, 3).ToArray();
            var sut = new CompositeBehaviorExpectation(expectations);

            var cmd = new DelegatingGuardClauseCommand();
            // Exercise system
            sut.Verify(cmd);
            // Verify outcome
            Assert.Equal(expectations.Length, observedCommands.Count(c => cmd.Equals(c)));
            // Teardown
        }
    }
}
