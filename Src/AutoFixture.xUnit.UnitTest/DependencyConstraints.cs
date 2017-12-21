using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.Xunit.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void AutoFixtureXunitDoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).Assembly.GetReferencedAssemblies();
            // Assert
            Assert.False(references.Any(an => an.Name == assemblyName));
        }

        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void AutoFixtureXunitUnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.False(references.Any(an => an.Name == assemblyName));
        }
    }
}
