using System.Collections.Generic;
using System.Linq;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class CompositeBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Arrange
            // Act
            var sut = new CompositeBehaviorExpectation();
            // Assert
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
        }

        [Fact]
        public void ConstructedWithArrayBehaviorExpectationsIsCorrect()
        {
            // Arrange
            var expectations = new[] { new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation() };
            var sut = new CompositeBehaviorExpectation(expectations);
            // Act
            IEnumerable<IBehaviorExpectation> result = sut.BehaviorExpectations;
            // Assert
            Assert.True(expectations.SequenceEqual(result));
        }

        [Fact]
        public void ConstructedWithEnumerableBehaviorExpectationsIsCorrect()
        {
            // Arrange
            var expectations = new[] { new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation() }.Cast<IBehaviorExpectation>();
            var sut = new CompositeBehaviorExpectation(expectations);
            // Act
            var result = sut.BehaviorExpectations;
            // Assert
            Assert.True(expectations.SequenceEqual(result));
        }

        [Fact]
        public void VerifyVerifiesAllBehaviorExpectations()
        {
            // Arrange
            var observedCommands = new List<IGuardClauseCommand>();
            var expectations = Enumerable.Repeat(new DelegatingBehaviorExpectation { OnVerify = observedCommands.Add }, 3).ToArray();
            var sut = new CompositeBehaviorExpectation(expectations);

            var cmd = new DelegatingGuardClauseCommand();
            // Act
            sut.Verify(cmd);
            // Assert
            Assert.Equal(expectations.Length, observedCommands.Count(c => cmd.Equals(c)));
        }
    }
}
