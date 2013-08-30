using System.Reflection;
using Ploeh.AutoFixture.NUnit.Addins.Builders;
using Xunit;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    public class AutoTestCaseProviderTest
    {
        private MethodInfo method;

        public AutoTestCaseProviderTest()
        {
            this.method = typeof(FakeAutoTestCase).GetMethod("DoSomething");
        }

        [Fact]
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

        [Fact]
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