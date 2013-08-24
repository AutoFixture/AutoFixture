using System;
using System.Linq;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org.Addins;
using Ploeh.AutoFixture.NUnit.org.Addins.Builders;
using Ploeh.TestTypeFoundation;

namespace AutoFixture.NUnit.org.Addins.UnitTest
{
    [TestFixture]
    public class AutoTestCaseBuilderTest
    {
        private MethodInfo _method;
        private Type[] _parameterTypes;

        private AutoTestCaseBuilder _builder;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            //CoreExtensions.Host.InstallAdhocExtensions(typeof(AutoDataAddin).Assembly);

            _method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            _parameterTypes = _method.GetParameters().Select(o => o.ParameterType).ToArray();
        }

        [SetUp]
        public void SetUp()
        {
            _builder = new AutoTestCaseBuilder();   
        }

        [Test]
        public void CanBuildFromAutoDataTestCaseBuilder()
        {
            var actual = _builder.CanBuildFrom(_method);

            Assert.IsTrue(actual);
        }

        [Test]
        public void BuildFromAutoDataTestCaseBuilderReturnsCorrectly()
        {
            var actual = _builder.BuildFrom(_method);
        }
    }
}