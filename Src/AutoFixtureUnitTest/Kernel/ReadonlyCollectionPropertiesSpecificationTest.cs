using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ReadonlyCollectionPropertiesSpecificationTest
    {
        [Fact]
        public void SutIsSpecification()
        {
            // Arrange
            // Act
            var sut = new ReadonlyCollectionPropertiesSpecification();

            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(0)]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(NonCompliantCollectionHolder<string>))]
        [InlineData(typeof(NonCompliantCollectionHolder<int>))]
        [InlineData(typeof(NonCompliantCollectionHolder<object>))]
        [InlineData(typeof(List<CollectionHolder<string>>))]
        [InlineData(typeof(List<CollectionHolder<int>>))]
        [InlineData(typeof(List<CollectionHolder<object>>))]
        public void IsSatisfiedByReturnsCorrectResultForNonReadonlyCollectionPropertiesRequest(object request)
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesSpecification();

            // Act
            var result = sut.IsSatisfiedBy(request);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(CollectionHolder<string>))]
        [InlineData(typeof(CollectionHolder<int>))]
        [InlineData(typeof(CollectionHolder<object>))]
        public void IsSatisfiedByReturnsCorrectResultForReadonlyCollectionPropertiesRequest(Type request)
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesSpecification();

            // Act
            var result = sut.IsSatisfiedBy(request);

            // Assert
            Assert.True(result);
        }
    }
}