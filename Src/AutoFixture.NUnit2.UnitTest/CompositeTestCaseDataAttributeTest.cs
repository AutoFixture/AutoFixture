using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class CompositeTestCaseDataAttributeTest
    {
        [Test]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeTestCaseDataAttribute();
            // Verify outcome
            Assert.IsInstanceOf<TestCaseDataAttribute>(sut);
            // Teardown
        }

        [Test]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeTestCaseDataAttribute(null));
            // Teardown
        }

        [Test]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            
            var attributes = new[]
            {
                new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeTestCaseDataAttribute(attributes);
            // Exercise system
            IEnumerable<TestCaseDataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Test]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeTestCaseDataAttribute((IEnumerable<TestCaseDataAttribute>)null));
            // Teardown
        }

        [Test]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            
            var attributes = new[]
            {
                new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeTestCaseDataAttribute(attributes);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Test]
        public void GetArgumentsWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeTestCaseDataAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetArguments(null).ToList());
            // Teardown
        }

        [Test]
        public void GetArgumentsOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            
            var sut = new CompositeTestCaseDataAttribute(
               new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeTestCaseDataAttribute(method, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var result = sut.GetArguments(a.Method);
            Array.ForEach(result.ToArray(), Assert.IsEmpty);
            // Teardown
        }
    }
}