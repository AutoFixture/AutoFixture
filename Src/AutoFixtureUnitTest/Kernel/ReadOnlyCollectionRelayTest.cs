using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class ReadOnlyCollectionRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new ReadOnlyCollectionRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new ReadOnlyCollectionRelay();
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
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(IEnumerable<Version>))]
        public void CreateWithNonListRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new ReadOnlyCollectionRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(IReadOnlyCollection<object>), typeof(object))]
        [InlineData(typeof(IReadOnlyCollection<string>), typeof(string))]
        [InlineData(typeof(IReadOnlyCollection<int>), typeof(int))]
        [InlineData(typeof(IReadOnlyCollection<Version>), typeof(Version))]
        public void CreateWithListRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Arrange
            var expectedRequest = typeof(List<>).MakeGenericType(itemType);
            object contextResult = typeof(List<>).MakeGenericType(itemType).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? contextResult : new NoSpecimen() };

            var sut = new ReadOnlyCollectionRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(contextResult, result);
        }
    }
}
