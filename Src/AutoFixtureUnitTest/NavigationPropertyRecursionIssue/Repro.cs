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
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Exercise system
            var session = fixture.Create<Session>();
            // Verify outcome
            Assert.Empty(session.Language.Sessions);
            // Teardown
        }
    }
}
