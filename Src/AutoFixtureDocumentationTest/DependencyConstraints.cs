using System.Linq;
using System.Reflection;
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
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}
