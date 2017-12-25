using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class DictionarySpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new DictionarySpecification();
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
        public void IsSatisfiedByNonDictionaryRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new DictionarySpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(Dictionary<object, object>))]
        [InlineData(typeof(Dictionary<int, string>))]
        [InlineData(typeof(Dictionary<string, int>))]
        [InlineData(typeof(Dictionary<Version, ConcreteType>))]
        public void IsSatisfiedByDictionaryRequestReturnsCorrectResult(Type request)
        {
            // Arrange
            var sut = new DictionarySpecification();
            // Act
            var result = sut.IsSatisfiedBy(request);
            // Assert
            Assert.True(result);
        }
    }
}
