using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    public class CompositeDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeTestCaseDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<TestCaseDataAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeTestCaseDataAttribute(null));
            // Teardown
        }

        [Fact]
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

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeTestCaseDataAttribute((IEnumerable<TestCaseDataAttribute>)null));
            // Teardown
        }

        [Fact]
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

        [Fact]
        public void GetArgumentsWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeTestCaseDataAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetArguments(null).ToList());
            // Teardown
        }

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
            Array.ForEach(result.ToArray(), Assert.Empty);
            // Teardown
        }
    }
}