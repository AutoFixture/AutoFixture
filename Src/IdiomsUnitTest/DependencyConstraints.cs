using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void IdiomsDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(IIdiomaticAssertion).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }

        [Theory]
        [InlineData("Moq")]
        [InlineData("Rhino.Mocks")]
        public void IdiomsUnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }
    }
}
