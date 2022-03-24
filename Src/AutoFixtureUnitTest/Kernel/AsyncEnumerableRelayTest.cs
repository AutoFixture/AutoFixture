using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class AsyncEnumerableRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange & Act
            var sut = new AsyncEnumerableRelay();

            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void RequestsWithNullContextThrow()
        {
            // Arrange
            var sut = new AsyncEnumerableRelay();
            var dummyRequest = new object();

            // Act & Assert
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
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(IEnumerable<>))]
        public void NonAsyncEnumerableRequestsReturnNoSpecimen(object request)
        {
            // Arrange
            var sut = new AsyncEnumerableRelay();

            // Act
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = _ => Enumerable.Empty<object>()
            };
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Theory]
        [InlineData(typeof(IAsyncEnumerable<object>), typeof(object))]
        [InlineData(typeof(IAsyncEnumerable<string>), typeof(string))]
        [InlineData(typeof(IAsyncEnumerable<int>), typeof(int))]
        [InlineData(typeof(IAsyncEnumerable<Version>), typeof(Version))]
        public void AsyncEnumerableRequestsReturnExpectedSequence(Type request, Type itemType)
        {
            // Arrange
            var expectedRequest = new MultipleRequest(itemType);
            object contextResult = Enumerable.Empty<object>();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r)
                    ? contextResult
                    : new NoSpecimen()
            };
            var sut = new AsyncEnumerableRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.IsAssignableFrom(request, result);
        }

        [Fact]
        public async Task MultipleResultsConvertedToRequestedType()
        {
            // Arrange
            var request = typeof(IAsyncEnumerable<int>);
            var expectedRequest = new MultipleRequest(typeof(int));
            var enumerable = Enumerable.Range(1, 3);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r)
                    ? enumerable.Cast<object>()
                    : new NoSpecimen()
            };
            var sut = new AsyncEnumerableRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            var actual = await Assert.IsAssignableFrom<IAsyncEnumerable<int>>(result)
                .ToListAsync().ConfigureAwait(false);
            Assert.Equal(enumerable, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(object[]))]
        public void InvalidResultForMultipleRequestReturnsNoSpecimen(object response)
        {
            // Arrange
            var request = typeof(IAsyncEnumerable<object>);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => response
            };
            var sut = new AsyncEnumerableRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public async Task MultipleRequestResultsFilterOmitSpecimen()
        {
            // Arrange
            var request = typeof(IAsyncEnumerable<int>);
            var expectedRequest = new MultipleRequest(typeof(int));

            var enumerable = new object[] { 1, new OmitSpecimen(), 3 };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r)
                    ? enumerable
                    : new NoSpecimen()
            };

            var sut = new AsyncEnumerableRelay();

            // Act
            var result = sut.Create(request, context);

            // Assert
            var actual = await Assert.IsAssignableFrom<IAsyncEnumerable<int>>(result)
                .ToListAsync().ConfigureAwait(false);
            Assert.Equal(enumerable.OfType<int>(), actual);
        }
    }
}