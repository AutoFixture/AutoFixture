using NUnit.Framework;
using System.Linq;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;
using System;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class InlineAutoDataAttributeTest
    {
        [Test]
        public void IfArguementsIsNullThenThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataAttribute(null));
        }

        [Test]
        public void IfExtendedWithNullFixtureThenThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataAttributeStub(null));
        }

        [Test]
        public void IfCreateParametersThrowsExceptionThenReturnsNotRunnableTestMethodWithExceptionInfoAsSkipReason()
        {
            // Arrange
            // DummyFixture is set up to throw DummyException when invoked by AutoDataAttribute
            var inlineAutoDataAttributeStub = new InlineAutoDataAttributeStub(new ThrowingStubFixture());

            var fixtureType = this.GetType();

            var methodWrapper = new MethodWrapper(fixtureType, fixtureType.GetMethod("DummyTestMethod"));
            var testSuite = new TestSuite(fixtureType);

            // Act
            var testMethod = inlineAutoDataAttributeStub.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.That(testMethod.RunState == RunState.NotRunnable);

            Assert.That(testMethod.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo(ExceptionHelper.BuildMessage(new ThrowingStubFixture.DummyException())));
        }

        [Test]
        public void InitializedWithArgumentsHasCorrectArguments()
        {
            // Fixture setup
            var expectedArguments = new object[] { };
            var sut = new InlineAutoDataAttribute(expectedArguments);
            // Exercise system
            var result = sut.Arguments;
            // Verify outcome
            Assert.AreSame(expectedArguments, result);
            // Teardown
        }

        /// <summary>
        /// This is used in BuildFromYieldsParameterValues for building a unit test method
        /// </summary>
        public void DummyTestMethod(int anyInt, double anyDouble)
        {
        }
    }
}
