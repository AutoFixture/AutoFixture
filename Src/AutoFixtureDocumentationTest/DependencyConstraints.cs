using System.Reflection;
using Xunit;

namespace AutoFixtureDocumentationTest
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
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }
    }
}
