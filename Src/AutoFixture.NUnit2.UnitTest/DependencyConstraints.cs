using System.Linq;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class DependencyConstraints
    {
        [TestCase("Moq")]
        [TestCase("Rhino.Mocks")]
        public void AutoFixtureXunitDoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).Assembly.GetReferencedAssemblies();
            // Assert
            Assert.False(references.Any(an => an.Name == assemblyName));
        }

        [TestCase("Moq")]
        [TestCase("Rhino.Mocks")]
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
