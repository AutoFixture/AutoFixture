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
            // Arrange
            // Act
            var sut = new AutoDataProvider();
            var actual = sut.HasTestCasesFor(this.method);
            // Assert
            Assert.True(actual);
        }

        [Test]
        public void GetTestCasesForAutoDataBuilderReturnsCorrectly()
        {
            // Arrange
            // Act
            var sut = new AutoDataProvider();
            var actual = sut.GetTestCasesFor(this.method);
            // Assert
            Assert.NotNull(actual);
        }
    }
}