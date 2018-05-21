using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.Xunit.UnitTest
{
    public class CompositeDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Arrange
            // Act
            var sut = new CompositeDataAttribute();
            // Assert
            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Arrange
            Action a = () => { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Assert
            Assert.True(attributes.SequenceEqual(result));
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute((IEnumerable<DataAttribute>)null));
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            Action a = () => { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.True(attributes.SequenceEqual(result));
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Arrange
            var sut = new CompositeDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null, dummyTypes).ToList());
        }

        [Fact]
        public void GetDataWithNullTypesThrows()
        {
            // Arrange
            var sut = new CompositeDataAttribute();
            Action a = () => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(a.Method, null).ToList());
        }

        [Fact]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Arrange
            Action a = () => { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()));

            // Act & assert
            var result = sut.GetData(a.Method, Type.EmptyTypes);
            Array.ForEach(result.ToArray(), Assert.Empty);
        }
    }
}