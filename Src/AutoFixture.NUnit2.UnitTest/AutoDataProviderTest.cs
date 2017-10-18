using System.Reflection;
using AutoFixture.NUnit2.Addins.Builders;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class AutoDataProviderTest
    {
        private readonly MethodInfo method;

        public AutoDataProviderTest()
        {
            this.method = typeof(FakeAutoDataFixture).GetMethod("DoSomething");
        }

        [Test]
        public void HasTestCasesForAutoDataProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataProvider();
            var actual = sut.HasTestCasesFor(this.method);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Test]
        public void GetTestCasesForAutoDataBuilderReturnsCorrectly()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataProvider();
            var actual = sut.GetTestCasesFor(this.method);
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown
        }
    }
}