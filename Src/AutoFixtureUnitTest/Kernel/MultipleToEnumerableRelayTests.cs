using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MultipleToEnumerableRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new MultipleToEnumerableRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(object))]
        [InlineData(typeof(Type))]
        [InlineData(1)]
        [InlineData(9992)]
        [InlineData("")]
        public void CreateFromNonMultipleRequestReturnsCorrectResult(
            object request)
        {
            // Arrange
            var sut = new MultipleToEnumerableRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(object), 1)]
        [InlineData(typeof(string), 42)]
        [InlineData(typeof(int), 1337)]
        public void CreateFromMultipleRequestReturnsCorrectResult(
            Type itemType,
            int arrayLength)
        {
            // Arrange
            var sut = new MultipleToEnumerableRelay();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(
                        typeof(IEnumerable<>).MakeGenericType(itemType),
                        r);
                    return Array.CreateInstance((Type)itemType, arrayLength);
                }
            };
            // Act
            var request = new MultipleRequest(itemType);
            var actual = sut.Create(request, context);
            // Assert
            Assert.IsAssignableFrom(
                typeof(IEnumerable<>).MakeGenericType(itemType),
                actual);
            var enumerable =
                Assert.IsAssignableFrom<System.Collections.IEnumerable>(actual);
            Assert.Equal(arrayLength, enumerable.Cast<object>().Count());
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("1")]
        [InlineData(false)]
        public void CreateFromMultipleRequestWithNonTypeRequestReturnsCorrectResult(
            object innerRequest)
        {
            // Arrange
            var sut = new MultipleToEnumerableRelay();
            var request = new MultipleRequest(innerRequest);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(bool), true, 53)]
        [InlineData(typeof(string), "ploeh", 9)]
        [InlineData(typeof(int), 7, 902)]
        public void CreateFromMultipleSeededRequestReturnsCorrectResult(
            Type itemType,
            object seed,
            int arrayLength)
        {
            // Arrange
            var sut = new MultipleToEnumerableRelay();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(
                        typeof(IEnumerable<>).MakeGenericType(itemType),
                        r);
                    return Array.CreateInstance((Type)itemType, arrayLength);
                }
            };
            // Act
            var request = new MultipleRequest(new SeededRequest(itemType, seed));
            var actual = sut.Create(request, context);
            // Assert
            Assert.IsAssignableFrom(
                typeof(IEnumerable<>).MakeGenericType(itemType),
                actual);
            var enumerable =
                Assert.IsAssignableFrom<System.Collections.IEnumerable>(actual);
            Assert.Equal(arrayLength, enumerable.Cast<object>().Count());
        }

        [Theory]
        [InlineData("bar")]
        [InlineData("9")]
        [InlineData(true)]
        public void CreateFromMultipleSeededRequestWithNonTypeRequestReturnsCorrectResult(
            object innerRequest)
        {
            // Arrange
            var sut = new MultipleToEnumerableRelay();
            var request = new MultipleRequest(
                new SeededRequest(
                    innerRequest,
                    new object()));
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new MultipleToEnumerableRelay();
            // Act & assert
            var dummyRequest = new object();
            Assert.Throws<ArgumentNullException>(
                () => sut.Create(dummyRequest, null));
        }
    }
}
