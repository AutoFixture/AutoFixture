using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class EnumerableRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new EnumerableRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new EnumerableRelay();
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
        [InlineData(typeof(IList<object>))]
        [InlineData(typeof(IList<string>))]
        [InlineData(typeof(IList<int>))]
        [InlineData(typeof(IList<Version>))]
        public void CreateWithNoneEnumerableRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new EnumerableRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext { OnResolve = r => Enumerable.Empty<object>() };
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(IEnumerable<object>), typeof(object))]
        [InlineData(typeof(IEnumerable<string>), typeof(string))]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [InlineData(typeof(IEnumerable<Version>), typeof(Version))]
        public void CreateWithEnumerableRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Arrange
            var expectedRequest = new MultipleRequest(itemType);
            object contextResult = Enumerable.Empty<object>();
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? contextResult : new NoSpecimen() };

            var sut = new EnumerableRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateConvertsEnumerableToCorrectGenericType()
        {
            // Arrange
            var request = typeof(IEnumerable<int>);
            var expectedRequest = new MultipleRequest(typeof(int));
            var enumerable = Enumerable.Range(1, 3).Cast<object>();
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)enumerable : new NoSpecimen() };

            var sut = new EnumerableRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            var e = Assert.IsAssignableFrom<IEnumerable<int>>(result);
            Assert.True(enumerable.Cast<int>().SequenceEqual(e));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(object[]))]
        public void CreateReturnsCorrectResultWhenContextReturnsNonEnumerableResult(object response)
        {
            // Arrange
            var request = typeof(IEnumerable<object>);
            var context = new DelegatingSpecimenContext { OnResolve = r => response };
            var sut = new EnumerableRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFiltersOmitSpecimenInstances()
        {
            // Arrange
            var request = typeof(IEnumerable<int>);
            var expectedRequest = new MultipleRequest(typeof(int));

            var enumerable = new object[]
            {
                1,
                new OmitSpecimen(),
                3
            };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? (object)enumerable : new NoSpecimen()
            };

            var sut = new EnumerableRelay();
            // Act
            var actual = sut.Create(request, context);
            // Assert
            var iter = Assert.IsAssignableFrom<IEnumerable<int>>(actual);
            Assert.True(
                enumerable.OfType<int>().SequenceEqual(iter),
                "Actual sequence is not equal to expected sequence.");
        }
    }
}
