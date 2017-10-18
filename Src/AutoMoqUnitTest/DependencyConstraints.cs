using System.Reflection;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Rhino.Mocks")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoMoqDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoMoqCustomization).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }

        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Rhino.Mocks")]
        public void AutoFixtureUnitTestsDoNotReference(string assemblyName)
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