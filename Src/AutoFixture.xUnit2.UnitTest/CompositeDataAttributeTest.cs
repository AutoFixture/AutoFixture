using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class CompositeDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<DataAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.GetMethodInfo();

            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
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
                new CompositeDataAttribute((IReadOnlyCollection<DataAttribute>)null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.GetMethodInfo();

            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null).ToList());
            // Teardown
        }

        [Fact]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.GetMethodInfo();
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var result = sut.GetData(a.GetMethodInfo());
            result.ToList().ForEach(Assert.Empty);
            // Teardown
        }
    }
}