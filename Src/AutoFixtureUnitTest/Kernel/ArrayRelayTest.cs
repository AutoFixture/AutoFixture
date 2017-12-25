using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ArrayRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new ArrayRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new ArrayRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void CreateWithNoneArrayRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new ArrayRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(object[]), typeof(object))]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(Version[]), typeof(Version))]
        public void CreateWithArrayRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Arrange
            var expectedRequest = new MultipleRequest(itemType);
            object expectedResult = Array.CreateInstance(itemType, 0);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new ArrayRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateConvertsEnumerableToArray()
        {
            // Arrange
            var request = typeof(int[]);
            var expectedRequest = new MultipleRequest(typeof(int));
            var enumerable = Enumerable.Range(1, 3);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)enumerable : new NoSpecimen() };

            var sut = new ArrayRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            var a = Assert.IsAssignableFrom<int[]>(result);
            Assert.True(enumerable.SequenceEqual(a));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(object[]))]
        public void CreateReturnsCorrectResultWhenContextReturnsNonEnumerableResult(object response)
        {
            // Arrange
            var request = typeof(object[]);
            var context = new DelegatingSpecimenContext { OnResolve = r => response };
            var sut = new ArrayRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}
