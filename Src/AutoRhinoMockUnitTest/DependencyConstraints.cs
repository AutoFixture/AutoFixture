using Xunit;

namespace AutoFixture.AutoRhinoMock.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Moq")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureDoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoRhinoMockCustomization).Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }

        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Moq")]
        public void AutoFixtureUnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}