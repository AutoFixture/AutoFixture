using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class AsyncEnumeratorRelayTests
    {
        public static IEnumerable<object[]> NonTypeRequests => new[]
        {
            new object[] { null },
            new object[] { new object() },
            new object[] { string.Empty },
            new object[] { 1 },
        };

        public static IEnumerable<object[]> InvalidTypeRequests => new[]
        {
            new object[] { typeof(object) },
            new object[] { typeof(string) },
            new object[] { typeof(int) },
            new object[] { typeof(Version) },
        };

        public static IEnumerable<object[]> ValidTypeRequests => new[]
        {
            new object[] { typeof(IAsyncEnumerator<object>), typeof(object) },
            new object[] { typeof(IAsyncEnumerator<string>), typeof(string) },
            new object[] { typeof(IAsyncEnumerator<int>), typeof(int) },
            new object[] { typeof(IAsyncEnumerator<Version>), typeof(Version) },
        };

        [Fact]
        public void SutIsISpecimenBuilder()
        {
            // Arrange
            var sut = new AsyncEnumeratorRelay();

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new AsyncEnumeratorRelay();
            var dummyRequest = new object();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Create(dummyRequest, null));
        }

        [Theory]
        [MemberData(nameof(NonTypeRequests))]
        public void NonTypeRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var sut = new AsyncEnumeratorRelay();
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var actual = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Theory]
        [MemberData(nameof(InvalidTypeRequests))]
        public void InvalidTypeRequestReturnsNoSpecimen(Type request)
        {
            // Arrange
            var sut = new AsyncEnumeratorRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var actual = sut.Create(request, dummyContext);
            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Theory]
        [MemberData(nameof(ValidTypeRequests))]
        public void ValidTypeRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Arrange
            var expectedRequest = typeof(IAsyncEnumerable<>)
                .MakeGenericType(itemType);
            var asyncEnumerable = typeof(AsyncEnumerable)
                .GetMethod(nameof(AsyncEnumerable.Empty))
                .MakeGenericMethod(itemType)
                .Invoke(null, null);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r)
                    ? asyncEnumerable
                    : new NoSpecimen()
            };
            var sut = new AsyncEnumeratorRelay();

            // Act
            var actual = sut.Create(request, context);

            // Assert
            Assert.IsAssignableFrom(request, actual);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var sut = new AsyncEnumeratorRelay();
            var request = typeof(IAsyncEnumerator<int>);
            var collection = new[] { 54, 212, 376 };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(typeof(IAsyncEnumerable<int>))
                    ? collection.ToAsyncEnumerable()
                    : new NoSpecimen()
            };

            // Act
            var actual = sut.Create(request, context);

            // Assert
            Assert.IsAssignableFrom(request, actual);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenAsyncEnumerableIsNotResolved()
        {
            // Arrange
            var sut = new AsyncEnumeratorRelay();
            var request = typeof(IAsyncEnumerator<int>);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = _ => new NoSpecimen()
            };

            // Act
            var actual = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Fact]
        public void FixtureCanCreateAsyncEnumerator()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var actual = fixture.Create<IAsyncEnumerator<int>>();

            // Assert
            Assert.IsAssignableFrom<IAsyncEnumerator<int>>(actual);
        }

        [Fact]
        public async Task CreatedAsyncEnumeratorYieldsValues()
        {
            // Arrange
            var fixture = new Fixture();
            var enumerator = fixture.Create<IAsyncEnumerator<int>>();
            var collector = new List<int>();

            // Act
            while (await enumerator.MoveNextAsync())
            {
                collector.Add(enumerator.Current);
            }

            // Assert
            Assert.NotEmpty(collector);
            Assert.Equal(fixture.RepeatCount, collector.Count);
        }
    }
}
