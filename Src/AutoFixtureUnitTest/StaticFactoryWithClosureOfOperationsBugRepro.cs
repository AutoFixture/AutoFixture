using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    /* This class contains regression tests against this bug:
     * http://autofixture.codeplex.com/workitem/4256 */
    public class StaticFactoryWithClosureOfOperationsBugRepro
    {
        [Fact]
        public void CreateWithOmitOnRecursionThrowsAppropriateException()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<ClosureOfOperationsHost>());
        }

        private class ClosureOfOperationsHost
        {
            private ClosureOfOperationsHost()
            {
            }

            public static ClosureOfOperationsHost CloseAround(
                ClosureOfOperationsHost thing)
            {
                return thing;
            }
        }
    }
}
