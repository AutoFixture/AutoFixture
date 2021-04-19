using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TestTypeFoundation;

namespace AutoFixture.NUnit3.UnitTest
{
    public class AutoTestCaseSourceAttributeTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("\t\t\t")]
        [TestCase("\r\n")]
        [TestCase("\r")]
        [TestCase("SomeString")]
        public void CanCreateInstanceWithSourceName(string sourceName)
        {
            // Arrange
            TestDelegate act = () => _ = new AutoTestCaseSourceAttributeStub(sourceName);

            // Act & Assert
            Assert.DoesNotThrow(act);
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(PropertyHolder<string>))]
        public void CanCreateInstanceWithSourceType(Type sourceType)
        {
            // Arrange
            TestDelegate act = () => _ = new AutoTestCaseSourceAttributeStub(sourceType);

            // Act & Assert
            Assert.DoesNotThrow(act);
        }

        [Test]
        [TestCase(typeof(string), "")]
        [TestCase(typeof(PropertyHolder<string>), nameof(PropertyHolder<object>.Property))]
        public void CanCreateInstanceWithSourceTypeAndSourceName(Type sourceType, string sourceName)
        {
            // Arrange
            TestDelegate act = () => _ = new AutoTestCaseSourceAttributeStub(sourceType, sourceName);

            // Act & Assert
            Assert.DoesNotThrow(act);
        }

        [Test]
        public void ThrowsWhenExtendedWithNullFixture()
        {
            // Arrange
            TestDelegate act = () => _ = new AutoTestCaseSourceAttributeStub(
                null,
                typeof(TypeWithCustomizationAttributes),
                nameof(TypeWithCustomizationAttributes.CreateWithFrozenAndGreedy));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void FixtureFactoryNotInvokedImmediately()
        {
            // Arrange
            var factory = new DelegatingFixtureFactory();

            // Act
            var sut = new AutoTestCaseSourceAttributeStub(factory);

            // Assert
            Assert.False(factory.Invoked);
        }

        [Test]
        public void SkipsTestsWhenFixtureThrows()
        {
            // Arrange
            // DummyFixture is set up to throw DummyException when invoked by AutoDataAttribute
            var fixtureType = typeof(TestClassWithEnumerableSources);
            var testCasesMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestCasesMethod));
            var testMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestMethod));
            var methodWrapper = new MethodWrapper(fixtureType, testMethod);
            var testSuite = new TestSuite(fixtureType);
            var sut = new AutoTestCaseSourceAttributeStub(
                () => new ThrowingStubFixture(),
                fixtureType, testCasesMethod?.Name);

            // Act
            var actual = sut.BuildFrom(methodWrapper, testSuite).Skip(1).First();

            // Assert
            Assert.That(actual.RunState == RunState.NotRunnable);
        }

        [Test]
        public void DoesNotActivateFixtureFactoryWhenSourceProvidesSufficientArguments()
        {
            // Arrange
            var factory = new DelegatingFixtureFactory();
            var fixtureType = typeof(TestClassWithEnumerableSources);
            var testCasesMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestCasesMethod));
            var testMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestMethod));
            var methodWrapper = new MethodWrapper(fixtureType, testMethod);
            var testSuite = new TestSuite(fixtureType);
            var sut = new AutoTestCaseSourceAttributeStub(factory, fixtureType, testCasesMethod?.Name);

            // Act
            _ = sut.BuildFrom(methodWrapper, testSuite).First();

            // Assert
            Assert.False(factory.Invoked);
        }

        [Test]
        public void SetsExpectedTestCaseSource()
        {
            // Arrange
            var fixtureType = typeof(TestClassWithEnumerableSources);
            var testCasesMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestCasesMethod));
            var testMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestMethod));
            var methodWrapper = new MethodWrapper(fixtureType, testMethod);
            var testSuite = new TestSuite(fixtureType);
            var sut = new AutoTestCaseSourceAttribute(fixtureType, testCasesMethod?.Name);

            // Act
            var actual = sut.BuildFrom(methodWrapper, testSuite).ToList();

            Assert.True(actual.All(x => x.GetArguments().Length == methodWrapper.GetParameters().Length));
        }

        [Test]
        public void SetsExpectedTestCaseSourceForLocalSourceMethod()
        {
            // Arrange
            var fixtureType = typeof(TestClassWithEnumerableSources);
            var testCasesMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestCasesMethod));
            var testMethod = fixtureType.GetMethod(nameof(TestClassWithEnumerableSources.TestMethod));
            var methodWrapper = new MethodWrapper(fixtureType, testMethod);
            var testSuite = new TestSuite(fixtureType);
            var sut = new AutoTestCaseSourceAttribute(testCasesMethod?.Name);

            // Act
            var actual = sut.BuildFrom(methodWrapper, testSuite).ToList();

            Assert.True(actual.All(x => x.GetArguments().Length == methodWrapper.GetParameters().Length));
        }
    }
}