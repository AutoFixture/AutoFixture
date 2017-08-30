using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeTest
    {
        [Test]
        public void ExtendsAttribute()
        {
            Assert.That(new AutoDataAttribute(), Is.InstanceOf<Attribute>());
        }

        [Test]
        public void IfExtendedWithNullFixtureThenThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new AutoDataAttributeStub(null));
        }

        [Test]
        public void ImplementsITestBuilder()
        {
            var autoDataFixture = new AutoDataAttribute();
            Assert.That(autoDataFixture, Is.InstanceOf<ITestBuilder>());
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

        [Test]
        public void CanBeExtendedToTakeAnIFixture()
        {
            var stub = new AutoDataAttributeStub(new ThrowingStubFixture());

            Assert.That(stub, Is.AssignableTo<AutoDataAttribute>());
        }

        [Test]
        public void IfCreateParametersThrowsExceptionThenReturnsNotRunnableTestMethodWithExceptionInfoAsSkipReason()
        {
            // Arrange
            // DummyFixture is set up to throw DummyException when invoked by AutoDataAttribute
            var autoDataAttributeStub = new AutoDataAttributeStub(new ThrowingStubFixture());

            var fixtureType = this.GetType();

            var methodWrapper = new MethodWrapper(fixtureType, fixtureType.GetMethod("DummyTestMethod"));
            var testSuite = new TestSuite(fixtureType);

            // Act
            var testMethod = autoDataAttributeStub.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.That(testMethod.RunState == RunState.NotRunnable);

            Assert.That(testMethod.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo(ExceptionHelper.BuildMessage(new ThrowingStubFixture.DummyException())));
        }

        [TestCase("CreateWithFrozenAndFavorArrays")]
        [TestCase("CreateWithFavorArraysAndFrozen")]
        [TestCase("CreateWithFrozenAndFavorEnumerables")]
        [TestCase("CreateWithFavorEnumerablesAndFrozen")]
        [TestCase("CreateWithFrozenAndFavorLists")]
        [TestCase("CreateWithFavorListsAndFrozen")]
        [TestCase("CreateWithFrozenAndGreedy")]
        [TestCase("CreateWithGreedyAndFrozen")]
        [TestCase("CreateWithFrozenAndModest")]
        [TestCase("CreateWithModestAndFrozen")]
        [TestCase("CreateWithFrozenAndNoAutoProperties")]
        [TestCase("CreateWithNoAutoPropertiesAndFrozen")]
        public void GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Fixture setup
            var method = new MethodWrapper(typeof(TypeWithCustomizationAttributes), methodName);
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new AutoDataAttributeStub(fixture);
            // Exercise system
            sut.BuildFrom(method, new TestSuite(this.GetType())).Single();
            // Verify outcome
            Assert.False(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.True(customizationLog[1] is FreezeOnMatchCustomization);
            // Teardown
        }
    }
}
