using System.Reflection;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Foq")]
        [InlineData("FsCheck")]
        [InlineData("Moq")]
        [InlineData("NSubstitute")]
        [InlineData("nunit.framework")]
        [InlineData("Rhino.Mocks")]
        [InlineData("Unquote")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureXunit2DoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }

        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Foq")]
        [InlineData("FsCheck")]
        [InlineData("Moq")]
        [InlineData("NSubstitute")]
        [InlineData("nunit.framework")]
        [InlineData("Rhino.Mocks")]
        [InlineData("Unquote")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureXunit2UnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}