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
            // Fixture setup
            // Exercise system
            var sut = new AutoDataAttribute();
            // Verify outcome
            Assert.That(sut.TestMethodBuilder,
                Is.TypeOf<FixedNameTestMethodBuilder>());
            // Teardown
        }

        [Test]
        public void InlineAutoDataAttributeUsesRealValueByDefault()
        {
            // Fixture setup
            // Exercise system
            var sut = new InlineAutoDataAttribute();
            // Verify outcome
            Assert.That(sut.TestMethodBuilder,
                Is.TypeOf<FixedNameTestMethodBuilder>());
            // Teardown
        }

        [Test]
        public void AutoDataUsesFixedValuesForTestName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.Name,
                Is.EqualTo(@"FixedNameDecoratedMethod(auto<Int32>,auto<MyClass>)"));
            // Teardown
        }

        [Test]
        public void AutoDataFixedNameUsesFixedValuesForTestFullName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"Ploeh.AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.FixedNameDecoratedMethod(auto<Int32>,auto<MyClass>)"));
            // Teardown
        }

        [Test]
        public void AutoDataVolatileNameUsesRealValuesForTestName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.Name,
                Is.EqualTo(@"VolatileNameDecoratedMethod(42,""foo"")"));
            // Teardown
        }

        [Test]
        public void AutoDataVolatileNameUsesRealValuesForTestFullName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.AutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"Ploeh.AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.VolatileNameDecoratedMethod(42,""foo"")"));
            // Teardown
        }

        [Test]
        public void InlineAutoDataUsesFixedValuesForTestName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.InlineFixedNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.Name,
                Is.EqualTo(@"InlineFixedNameDecoratedMethod(""alpha"",""beta"",auto<String>)"));
            // Teardown
        }

        [Test]
        public void InlineAutoDataFixedNameUsesFixedValuesForTestFullName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataFixedNameAttribute>(nameof(TestNameStrategiesFixture.InlineFixedNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"Ploeh.AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.InlineFixedNameDecoratedMethod(""alpha"",""beta"",auto<String>)"));
            // Teardown
        }

        [Test]
        public void InlineAutoDataVolatileNameUsesRealValuesForTestName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.InlineVolatileNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.Name,
                Is.EqualTo(@"InlineVolatileNameDecoratedMethod(""alpha"",""beta"",""foo"")"));
            // Teardown
        }

        [Test]
        public void InlineAutoDataVolatileNameUsesRealValuesForFullName()
        {
            // Fixture setup
            // Exercise system
            var testMethod = GetTestMethod<TestNameStrategiesFixture.InlineAutoDataVolatileNameAttribute>(nameof(TestNameStrategiesFixture.InlineVolatileNameDecoratedMethod));
            // Verify outcome
            Assert.That(testMethod.FullName,
                Is.EqualTo(@"Ploeh.AutoFixture.NUnit3.UnitTest.TestNameStrategiesFixture.InlineVolatileNameDecoratedMethod(""alpha"",""beta"",""foo"")"));
            // Teardown
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
