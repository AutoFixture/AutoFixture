using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class DependencyConstraints
    {
        [TestCase("FakeItEasy")]
        [TestCase("Foq")]
        [TestCase("FsCheck")]
        [TestCase("Moq")]
        [TestCase("NSubstitute")]
        [TestCase("Rhino.Mocks")]
        [TestCase("Unquote")]
        [TestCase("xunit")]
        [TestCase("xunit.extensions")]
        public void AutoFixtureNUnit3DoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoDataAttribute).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [TestCase("FakeItEasy")]
        [TestCase("Foq")]
        [TestCase("FsCheck")]
        [TestCase("Moq")]
        [TestCase("NSubstitute")]
        [TestCase("Rhino.Mocks")]
        [TestCase("Unquote")]
        [TestCase("xunit")]
        [TestCase("xunit.extensions")]
        public void AutoFixtureNUnit3UnitTestsDoNotReference(string assemblyName)
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
