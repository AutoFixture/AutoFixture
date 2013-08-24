using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org.Addins;
using Ploeh.AutoFixture.NUnit.org.Addins.Builders;

namespace AutoFixture.NUnit.org.Addins.UnitTest
{
    [TestFixture]
    public class AutoDataTestCaseBuilderTest
    {
        private MethodInfo _methodInfo;
        private AutoDataTestCaseBuilder _builder;

        [TestFixtureSetUp]
        public void LocalSetUp()
        {
            CoreExtensions.Host.InstallAdhocExtensions(typeof(AutoDataAddin).Assembly);

            _methodInfo = typeof(SampleTest).GetMethod("SampleMethod");
        }

        [SetUp]
        public void SetUp()
        {
            _builder = new AutoDataTestCaseBuilder();   
        }

        [Test]
        public void CanBuildFromAutoDataTestCaseBuilder()
        {
            var actual = _builder.CanBuildFrom(_methodInfo);

            Assert.IsTrue(actual);
        }

        [Test]
        public void BuildFromAutoDataTestCaseBuilderReturnsCorrectly()
        {
            var actual = _builder.BuildFrom(_methodInfo);
        }
    }
}