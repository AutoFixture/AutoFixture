using System.Reflection;
using Xunit;

namespace AutoFixtureDocumentationTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        public void AutoFixtureDocumentationTestsDoeNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}
