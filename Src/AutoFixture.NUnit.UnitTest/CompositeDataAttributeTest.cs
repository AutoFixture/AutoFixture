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
            var sut = new CompositeArgumentsAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<ArgumentsAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeArgumentsAttribute(null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeArgumentsAttribute(attributes);
            // Exercise system
            IEnumerable<ArgumentsAttribute> result = sut.Attributes;
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
                new CompositeArgumentsAttribute((IEnumerable<ArgumentsAttribute>)null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeArgumentsAttribute(attributes);
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
            var sut = new CompositeArgumentsAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetArguments(null, dummyTypes).ToList());
            // Teardown
        }

        [Fact]
        public void GetDataWithNullTypesThrows()
        {
            // Fixture setup
            var sut = new CompositeArgumentsAttribute();
            Action a = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetArguments(a.Method, null).ToList());
            // Teardown
        }

        [Fact]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeArgumentsAttribute(
               new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeArgumentsAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var result = sut.GetArguments(a.Method, Type.EmptyTypes);
            Array.ForEach(result.ToArray(), Assert.Empty);
            // Teardown
        }
    }
}