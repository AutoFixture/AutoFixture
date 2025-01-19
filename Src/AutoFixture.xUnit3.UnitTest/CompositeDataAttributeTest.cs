using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace AutoFixture.Xunit3.UnitTest
{
    public class CompositeDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Arrange & Act
            var sut = new CompositeDataAttribute();

            // Assert
            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new CompositeDataAttribute(null));
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Arrange
            var a = () => { };
            var method = a.GetMethodInfo();

            var attributes = new DataAttribute[]
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
            Assert.Throws<ArgumentNullException>(
                () => new CompositeDataAttribute((IReadOnlyCollection<DataAttribute>)null));
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            var a = () => { };
            var method = a.GetMethodInfo();

            var attributes = new DataAttribute[]
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
        public async Task GetDataWithNullMethodThrows()
        {
            // Arrange
            var sut = new CompositeDataAttribute();
            // Act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => sut.GetData(null!, new DisposalTracker()).AsTask());
        }

        [Fact]
        public async Task GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Arrange
            Action a = () => { };
            var method = a.GetMethodInfo();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()));

            // Act
            var result = await sut.GetData(a.GetMethodInfo()!, new DisposalTracker());

            // Assert
            Assert.All(result, row => Assert.Empty(row.GetData()));
        }
    }
}