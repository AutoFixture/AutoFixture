using System.Reflection;
using Ploeh.AutoFixture.NUnit.Addin.Builders;
using Xunit;

namespace Ploeh.AutoFixture.NUnit.Addin.UnitTest
{
    public class AutoTestCaseProviderTest
    {
        private MethodInfo method;

        public AutoTestCaseProviderTest()
        {
            this.method = typeof(FakeAutoTestCase).GetMethod("DoSomething");
        }

        [Fact]
        public void HasTestCasesForAutoDataTestCaseProvider()
        {
            var sut = new AutoTestCaseProvider();
            var actual = sut.HasTestCasesFor(this.method);

            Assert.True(actual);
        }

        [Fact]
        public void GetTestCasesForAutoDataTestCaseBuilderReturnsCorrectly()
        {
            var sut = new AutoTestCaseProvider();
            var actual = sut.GetTestCasesFor(this.method);

            Assert.NotNull(actual);
        }
    }
}