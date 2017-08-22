using System.Linq;
using System.Reflection;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void SemanticComparisonDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(Likeness<object, object>).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void SemanticComparisonUnitTestsDoNotReference(string assemblyName)
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
