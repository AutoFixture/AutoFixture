using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void IdiomsDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(FixtureExtensions).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
        [InlineData("Moq")]
        public void IdiomsUnitTestsDoNotReference(string assemblyName)
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
