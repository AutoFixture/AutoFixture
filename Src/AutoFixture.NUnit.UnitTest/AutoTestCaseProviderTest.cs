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
            var sut = new AutoTestCaseProvider();
            var actual = sut.HasTestCasesFor(this.method);

            Assert.True(actual);
        }

        [Fact]
        public void GetTestCasesForAutoTestCaseTestCaseBuilderReturnsCorrectly()
        {
            var sut = new AutoTestCaseProvider();
            var actual = sut.GetTestCasesFor(this.method);

            Assert.NotNull(actual);
        }
    }
}