using System.Linq;
using Xunit;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
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
            // Fixture setup
            // Exercise system
            var references = typeof(AutoDataAttribute).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
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
            // Fixture setup
            // Exercise system
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}