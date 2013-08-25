using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org.Builders;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    [TestFixture]
    public class AutoTestCaseBuilderTest
    {
        private MethodInfo _method;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _method = typeof(FakeTest).GetMethod("DoSomething");
        }

        [Test]
        public void CanBuildFromAutoDataTestCaseBuilder()
        {
            var sut = new AutoTestCaseBuilder();
            var actual = sut.CanBuildFrom(_method);

            Assert.IsTrue(actual);
        }

        [Test]
        public void BuildFromAutoDataTestCaseBuilderReturnsCorrectly()
        {
            var sut = new AutoTestCaseBuilder();
            var actual = sut.BuildFrom(_method);
        }
    }
}