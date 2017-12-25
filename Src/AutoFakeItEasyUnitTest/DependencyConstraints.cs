using System.Reflection;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Rhino.Mocks")]
        [InlineData("Moq")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFakeItEasyDoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoFakeItEasyCustomization).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }

        [Theory]
        [InlineData("Rhino.Mocks")]
        [InlineData("Moq")]
        public void AutoFakeItEasyUnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}
