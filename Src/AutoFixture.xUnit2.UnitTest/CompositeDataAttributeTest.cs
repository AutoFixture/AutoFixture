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
            Action a = delegate { };
            var method = a.GetMethodInfo();

            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
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
                new CompositeDataAttribute((IReadOnlyCollection<DataAttribute>)null));
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            Action a = delegate { };
            var method = a.GetMethodInfo();

            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
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
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null).ToList());
        }

        [Fact]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Arrange
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

            // Act & assert
            var result = sut.GetData(a.GetMethodInfo());
            result.ToList().ForEach(Assert.Empty);
        }
    }
}