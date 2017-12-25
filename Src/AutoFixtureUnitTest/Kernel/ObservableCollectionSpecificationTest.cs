using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class ObservableCollectionSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new ObservableCollectionSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
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
        public void IsSatisfiedByNonEnumerableRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new ObservableCollectionSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(Dictionary<string, string>))]
        [InlineData(typeof(Stack<int>))]
        [InlineData(typeof(Collection<Version>))]
        public void IsSatisfiedByEnumerableNonObservableRequestReturnsCorrectResult(Type request)
        {
            // Arrange
            var sut = new ObservableCollectionSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(ObservableCollection<object>))]
        [InlineData(typeof(ObservableCollection<string>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(ObservableCollection<Version>))]
        public void IsSatisfiedByEnumerableRequestReturnsCorrectResult(Type request)
        {
            // Arrange
            var sut = new ObservableCollectionSpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
