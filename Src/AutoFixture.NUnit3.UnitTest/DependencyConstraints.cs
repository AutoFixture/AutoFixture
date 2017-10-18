using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AutoFixture.NUnit3.UnitTest
{
    public class DependencyConstraints
    {
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
            // Fixture setup
            // Exercise system
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
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
            // Fixture setup
            // Exercise system
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}
