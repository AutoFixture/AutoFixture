using System.Reflection;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest
{
    public class DependencyConstraintsTests
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
        public void AutoFixtureXunit3DoesNotReference(string assemblyName)
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
        public void AutoFixtureXunit3UnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }
    }
}