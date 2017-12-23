using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest.NavigationPropertyRecursionIssue
{
    public class Repro
    {
        /// <summary>
        /// This test reproduces the issue reported at
        /// http://stackoverflow.com/q/12531920/126014
        /// </summary>
        [Fact]
        public void Issue()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Act
            var session = fixture.Create<Session>();
            // Assert
            Assert.Empty(session.Language.Sessions);
        }
    }
}
