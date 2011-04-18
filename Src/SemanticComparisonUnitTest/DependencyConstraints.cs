using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void SemanticComparisonDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(Likeness<object, object>).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void SemanticComparisonUnitTestsDoNotReference(string assemblyName)
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
