using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AutoFixture.NUnit3.UnitTest
{
    public class DependencyConstraints
    {
        [Test]
        [InlineAutoData("FakeItEasy")]
        [InlineAutoData("Foq")]
        [InlineAutoData("FsCheck")]
        [InlineAutoData("Moq")]
        [InlineAutoData("NSubstitute")]
        [InlineAutoData("Rhino.Mocks")]
        [InlineAutoData("Unquote")]
        [InlineAutoData("xunit")]
        [InlineAutoData("xunit.extensions")]
        public void AutoFixtureNUnit3DoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.False(references.Any(an => an.Name == assemblyName));
        }

        [Test]
        [InlineAutoData("FakeItEasy")]
        [InlineAutoData("Foq")]
        [InlineAutoData("FsCheck")]
        [InlineAutoData("Moq")]
        [InlineAutoData("NSubstitute")]
        [InlineAutoData("Rhino.Mocks")]
        [InlineAutoData("Unquote")]
        [InlineAutoData("xunit")]
        [InlineAutoData("xunit.extensions")]
        public void AutoFixtureNUnit3UnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.False(references.Any(an => an.Name == assemblyName));
        }
    }
}
