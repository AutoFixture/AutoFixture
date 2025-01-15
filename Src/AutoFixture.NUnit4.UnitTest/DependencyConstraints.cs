using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AutoFixture.NUnit4.UnitTest
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
        public void AutoFixtureNUnit4DoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.That(references.Any(an => an.Name == assemblyName), Is.False);
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
        public void AutoFixtureNUnit4UnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.That(references.Any(an => an.Name == assemblyName), Is.False);
        }
    }
}
