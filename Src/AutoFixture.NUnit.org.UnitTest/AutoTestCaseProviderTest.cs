using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org.Builders;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    [TestFixture]
    public class AutoTestCaseProviderTest
    {
        private MethodInfo _method;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _method = typeof(FakeTest).GetMethod("DoSomething");
        }

        [Test]
        public void HasTestCasesForAutoDataTestCaseProvider()
        {
            var sut = new AutoTestCaseProvider();
            var actual = sut.HasTestCasesFor(_method);

            Assert.IsTrue(actual);
        }

        [Test]
        public void GetTestCasesForAutoDataTestCaseBuilderReturnsCorrectly()
        {
            var sut = new AutoTestCaseProvider();
            var actual = sut.GetTestCasesFor(_method);
        }
    }
}