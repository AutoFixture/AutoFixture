using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.Addins.Builders;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    [TestFixture]
    public class AutoTestCaseProviderTest
    {
        private readonly MethodInfo method;

        public AutoTestCaseProviderTest()
        {
            this.method = typeof(FakeAutoTestCaseFixture).GetMethod("DoSomething");
        }

        [Test]
        public void HasTestCasesForAutoTestCaseTestCaseProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoTestCaseProvider();
            var actual = sut.HasTestCasesFor(this.method);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Test]
        public void GetTestCasesForAutoTestCaseTestCaseBuilderReturnsCorrectly()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoTestCaseProvider();
            var actual = sut.GetTestCasesFor(this.method);
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown
        }
    }
}