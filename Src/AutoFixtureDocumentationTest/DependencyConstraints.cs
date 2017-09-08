using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureDocumentationTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        public void AutoFixtureDocumentationTestsDoeNotReference(string assemblyName)
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
