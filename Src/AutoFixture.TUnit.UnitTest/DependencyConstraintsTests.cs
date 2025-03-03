using System.Reflection;

namespace AutoFixture.TUnit.UnitTest
{
    public class DependencyConstraintsTests
    {
        [Test]
        [Arguments("FakeItEasy")]
        [Arguments("Foq")]
        [Arguments("FsCheck")]
        [Arguments("Moq")]
        [Arguments("NSubstitute")]
        [Arguments("nunit.framework")]
        [Arguments("Rhino.Mocks")]
        [Arguments("Unquote")]
        [Arguments("xunit")]
        [Arguments("xunit.extensions")]
        public void AutoFixtureXunit3DoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
        }

        [Test]
        [Arguments("FakeItEasy")]
        [Arguments("Foq")]
        [Arguments("FsCheck")]
        [Arguments("Moq")]
        [Arguments("NSubstitute")]
        [Arguments("nunit.framework")]
        [Arguments("Rhino.Mocks")]
        [Arguments("Unquote")]
        [Arguments("xunit")]
        [Arguments("xunit.extensions")]
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