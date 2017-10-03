using System.Linq;
using System.Reflection;
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
            var references = typeof(AutoDataAttribute).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
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
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }
    }
}