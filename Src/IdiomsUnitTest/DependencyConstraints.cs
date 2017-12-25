using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void IdiomsDoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(IIdiomaticAssertion).Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }

        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void IdiomsUnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}
