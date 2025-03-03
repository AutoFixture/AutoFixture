using System.Reflection;
using System.Threading.Tasks;

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
        public async Task AutoFixtureXunit3DoesNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            await Assert.That(references).DoesNotContain(an => an.Name == assemblyName);
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
        public async Task AutoFixtureXunit3UnitTestsDoNotReference(string assemblyName)
        {
            // Arrange
            // Act
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Assert
            await Assert.That(references).DoesNotContain(an => an.Name == assemblyName);
        }
    }
}