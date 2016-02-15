using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeTest
    {
        [Test]
        public void ExtendsNUnitAttribute()
        {
            Assert.That(new AutoDataAttribute(), Is.InstanceOf<NUnitAttribute>());
        }

        [Test]
        public void ImplementsITestBuilderITestCaseDataAndIImplyFixture()
        {
            var autoDataFixture = new AutoDataAttribute();
            Assert.That(autoDataFixture, Is.InstanceOf<ITestBuilder>());
            Assert.That(autoDataFixture, Is.InstanceOf<ITestCaseData>());
            Assert.That(autoDataFixture, Is.InstanceOf<IImplyFixture>());
        }

        [Test]
        public void BuildFromYieldsRunnableTestMethod()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var fixtureType = this.GetType();

            var methodWrapper = new MethodWrapper(fixtureType, fixtureType.GetMethod("DummyTestMethod"));
            var testSuite = new TestSuite(fixtureType);

            // Act
            var testMethod = autoDataAttribute.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.That(testMethod.RunState == RunState.Runnable);
        }

        /// <summary>
        /// This is used in BuildFromYieldsParameterValues for building a unit test method
        /// </summary>
        public void DummyTestMethod(int anyInt, double anyDouble)
        {
        }
    }
}
