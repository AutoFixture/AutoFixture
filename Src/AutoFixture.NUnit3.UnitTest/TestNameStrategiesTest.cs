using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3.UnitTest
{
    public class TestNameStrategiesTest
    {
        [Test]
        public void AutoDataAttributeUsesRealValueByDefault()
        {
            // Arrange
            // Act
            var sut = new AutoDataAttribute();
            // Assert
            Assert.That(sut.TestMethodBuilder,
                Is.TypeOf<FixedNameTestMethodBuilder>());
        }

        [Test]
        public void InlineAutoDataAttributeUsesRealValueByDefault()
        {
            // Arrange
            // Act
            var sut = new InlineAutoDataAttribute();
            // Assert
            Assert.That(sut.TestMethodBuilder,
                Is.TypeOf<FixedNameTestMethodBuilder>());
        }

        [Test]
        public void AutoDataUsesFixedValuesForTestName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.Name,
                Is.EqualTo(@"FixedNameDecoratedMethod(auto<Int32>,auto<MyClass>)"));
        }

        [Test]
        public void AutoDataFixedNameUsesFixedValuesForTestFullName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.FixedNameDecoratedMethod(auto<Int32>,auto<MyClass>)"));
        }

        [Test]
        public void AutoDataVolatileNameUsesRealValuesForTestName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.Name,
                Is.EqualTo(@"VolatileNameDecoratedMethod(42,""foo"")"));
        }

        [Test]
        public void AutoDataVolatileNameUsesRealValuesForTestFullName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.VolatileNameDecoratedMethod(42,""foo"")"));
        }

        [Test]
        public void InlineAutoDataUsesFixedValuesForTestName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.InlineFixedNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.Name,
                Is.EqualTo(@"InlineFixedNameDecoratedMethod(""alpha"",""beta"",auto<String>)"));
        }

        [Test]
        public void InlineAutoDataFixedNameUsesFixedValuesForTestFullName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.InlineFixedNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.InlineFixedNameDecoratedMethod(""alpha"",""beta"",auto<String>)"));
        }

        [Test]
        public void InlineAutoDataVolatileNameUsesRealValuesForTestName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.InlineVolatileNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.Name,
                Is.EqualTo(@"InlineVolatileNameDecoratedMethod(""alpha"",""beta"",""foo"")"));
        }

        [Test]
        public void InlineAutoDataVolatileNameUsesRealValuesForFullName()
        {
            // Arrange
            // Act
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.InlineVolatileNameDecoratedMethod));
            // Assert
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.InlineVolatileNameDecoratedMethod(""alpha"",""beta"",""foo"")"));
        }

        private static TestMethod GetTestMethod<TAttribute>(string testName) where TAttribute : Attribute, NUnit.Framework.Interfaces.ITestBuilder
        {
            var method = new MethodWrapper(typeof(TestNameStrategiesFixture), testName);
            var inlineAttribute = (TAttribute)method.MethodInfo.GetCustomAttribute(typeof(TAttribute));
            var testMethod = inlineAttribute.BuildFrom(method, null).Single();
            return testMethod;
        }
    }
}
