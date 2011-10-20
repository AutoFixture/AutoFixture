using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoMoqDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoFakeItEasyCustomization).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
        [InlineData("FakeItEasy")]
        public void AutoFixtureUnitTestsDoNotReference(string assemblyName)
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
