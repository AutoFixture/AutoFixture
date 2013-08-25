using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit;

namespace Ploe.AutoFixture.NUnit.UnitTest
{
    [TestFixture]
    public class DependencyConstraints
    {
        [Test]
        [TestCase("Moq")]
        [TestCase("Rhino.Mocks")]
        public void AutoFixtureXunitDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoTestCaseAttribute).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Test]
        [TestCase("Moq")]
        [TestCase("Rhino.Mocks")]
        public void AutoFixtureXunitUnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}
