using System.Reflection;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureDoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(Fixture).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }

        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void AutoFixtureUnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}
