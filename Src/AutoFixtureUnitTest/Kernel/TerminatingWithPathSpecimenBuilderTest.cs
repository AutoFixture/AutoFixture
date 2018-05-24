using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TerminatingWithPathSpecimenBuilderTest
    {
        [Fact]
        public void SutIsISpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new TerminatingWithPathSpecimenBuilder(new DelegatingSpecimenBuilder());
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void SutIsISpecimenBuilderNode()
        {
            // Arrange
            // Act
            var sut = new TerminatingWithPathSpecimenBuilder(new DelegatingSpecimenBuilder());
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new TerminatingWithPathSpecimenBuilder(null));
        }

        [Fact]
        public void SpecimenBuilderIsCorrect()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            // Act
            var sut = new TerminatingWithPathSpecimenBuilder(expected);
            // Assert
            Assert.Same(expected, sut.Builder);
        }

        [Fact]
        public void SutYieldsCorrectSequence()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(expected);
            // Act
            // Assert
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, sut.Cast<object>().Single());
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var tw = Assert.IsAssignableFrom<TerminatingWithPathSpecimenBuilder>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(tw.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var tw = Assert.IsAssignableFrom<TerminatingWithPathSpecimenBuilder>(actual);
            Assert.Equal(expected, tw.Builder);
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Arrange
            var expectedSpecimen = new object();
            var stubBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expectedSpecimen
            };

            var sut = new TerminatingWithPathSpecimenBuilder(stubBuilder);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }

        [Fact]
        public void CreateWillInvokeDecoratedBuilderWithCorrectParameters()
        {
            // Arrange
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var mockBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => verified = expectedRequest == r && expectedContainer == c
            };

            var sut = new TerminatingWithPathSpecimenBuilder(mockBuilder);
            // Act
            sut.Create(expectedRequest, expectedContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void SpecimenRequestsWillInitiallyBeEmpty()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Act & assert
            Assert.Empty(sut.SpecimenRequests);
        }

        [Fact]
        public void SpecimenRequestsRaisedFromTracerAreRecordedCorrectly()
        {
            // Arrange
            var requests = new[] { new object(), new object(), new object() };
            var requestQueue = new Queue<object>(requests);
            var firstRequest = requestQueue.Dequeue();

            object[] capturedRequests = null;
            TerminatingWithPathSpecimenBuilder sut = null;

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                {
                    if (requestQueue.Count > 0)
                        return c.Resolve(requestQueue.Dequeue());

                    capturedRequests = sut.SpecimenRequests.ToArray();
                    return new object();
                }
            };

            sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Cause sut to be executed recursively for multiple times.
            var context = new SpecimenContext(sut);

            // Act
            sut.Create(firstRequest, context);

            // Assert
            Assert.NotNull(capturedRequests);
            Assert.Equal(requests, capturedRequests);
        }

        [Fact]
        public void SpecimenRequestsAreEmptyWhenAllThatWereRequestedAreCreated()
        {
            // Arrange
            var requests = new[] { new object(), new object(), new object() };
            var requestQueue = new Queue<object>(requests);
            var firstRequest = requestQueue.Dequeue();

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => requestQueue.Count > 0
                    ? c.Resolve(requestQueue.Dequeue())
                    : new object()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Cause sut to be executed recursively for multiple times.
            var context = new SpecimenContext(sut);

            // Act
            sut.Create(firstRequest, context);

            // Assert
            Assert.Empty(sut.SpecimenRequests);
        }

        [Fact]
        public void SpecimenRequestsAreEmptyAfterThrowing()
        {
            // Arrange
            var requests = new[] { new object(), new object(), new object() };
            var requestQueue = new Queue<object>(requests);
            var firstRequest = requestQueue.Dequeue();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => requestQueue.Count > 0
                    ? c.Resolve(requestQueue.Dequeue())
                    : new NoSpecimen()
            };
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Cause sut to be executed recursively for multiple times.
            var container = new SpecimenContext(sut);

            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(firstRequest, container));

            Assert.Empty(sut.SpecimenRequests);
        }

        [Fact]
        public void CreateThrowsWhenNoSpecimenIsReturnedFromTheDecoratedGraph()
        {
            // Arrange
            var requests = new[] { new object(), new object(), new object() };
            var builder = new DelegatingSpecimenBuilder
            {
                // Returns NoSpecimen only on the last specimen request
                OnCreate = (r, c) => r == requests[2] ? new NoSpecimen() : new object()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var container = new SpecimenContext(sut);
            // Act & assert
            Assert.Null(Record.Exception(() => sut.Create(requests[0], container)));
            Assert.Null(Record.Exception(() => sut.Create(requests[1], container)));
            Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(requests[2], container));
        }

        [Theory]
        [InlineData(typeof(IInterface), "interface")]
        [InlineData(typeof(AbstractType), "abstract")]
        public void CreateThrowsWithAutoMockingHintOnInterfaceOrAbcRequest(
            object request,
            string requestType)
        {
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => new NoSpecimen()
            };
            var sut = new TerminatingWithPathSpecimenBuilder(builder);

            var context = new SpecimenContext(sut);
            var e = Assert.ThrowsAny<ObjectCreationException>(
                () => sut.Create(request, context));

            Assert.Contains(
                requestType,
                e.Message,
                StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains(
                "auto-mocking",
                e.Message,
                StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains(
                "request path",
                e.Message,
                StringComparison.CurrentCultureIgnoreCase);
        }

        [Fact]
        public void CreateOnMultipleThreadsConcurrentlyWorks()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => 99
            };
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r => 99
            };
            // Act
            int[] specimens = Enumerable.Range(0, 1000)
                .AsParallel()
                    .WithDegreeOfParallelism(8)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x => (int)sut.Create(typeof(int), dummyContext))
                .ToArray();
            // Assert
            Assert.Equal(1000, specimens.Length);
            Assert.True(specimens.All(s => s == 99));
        }

        [Fact]
        public void ObjectCreationExceptionIsThrownIfCreationFails()
        {
            // Arrange
            var request = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw new Exception()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(request, context));
        }

        [Fact]
        public void ExceptionWithCorrectMessageIsThrownIfCreationFails()
        {
            // Arrange
            var request = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw new Exception()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Act & assert
            var actualException = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(request, context));
            Assert.Contains("failed with exception", actualException.Message);
        }

        [Fact]
        public void InnerBuilderExceptionIsWrappedByCreationException()
        {
            // Arrange
            var request = new object();
            var exception = new Exception();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw exception
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(request, context));
            Assert.Same(exception, actualEx.InnerException);
        }

        [Fact]
        public void NestedExceptionIsWrappedForOneTimeOnly()
        {
            // Arrange
            var requests = new[] { new object(), new object(), new object() };
            var requestQueue = new Queue<object>(requests);
            var firstRequest = requestQueue.Dequeue();

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => requestQueue.Count > 0
                    ? c.Resolve(requestQueue.Dequeue())
                    : throw new InvalidOperationException()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Cause sut to be executed recursively for multiple times.
            var context = new SpecimenContext(sut);

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(firstRequest, context));
            Assert.IsAssignableFrom<InvalidOperationException>(actualEx.InnerException);
        }

        [Fact]
        public void ShouldWrapObjectCreationExceptionsFromInnerBuilders()
        {
            // Arrange
            var request = new object();
            var exceptionToThrow = new ObjectCreationException("Creation failed.");

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw exceptionToThrow
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(request, context));
            Assert.NotEqual(exceptionToThrow, actualEx);
        }

        [Fact]
        public void NestedObjectCreationExceptionIsWrappedForOneTimeOnly()
        {
            // Arrange
            var requests = new[] { new object(), new object(), new object() };
            var requestQueue = new Queue<object>(requests);
            var firstRequest = requestQueue.Dequeue();

            var expectedInnerException = new ObjectCreationException();

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => requestQueue.Count > 0
                    ? c.Resolve(requestQueue.Dequeue())
                    : throw expectedInnerException
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Cause sut to be executed recursively for multiple times.
            var context = new SpecimenContext(sut);

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(firstRequest, context));
            Assert.Equal(expectedInnerException, actualEx.InnerException);
        }

        [Fact]
        public void ThrownExceptionIncludesInnerExceptionMessages()
        {
            // Arrange
            var innerInnerException = new InvalidOperationException("INNER_INNER_EXCEPTION");
            var innerException = new InvalidOperationException("WRAPPED_INNER_EXCEPTION", innerInnerException);

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw innerException
            };

            var request = new object();
            var context = new DelegatingSpecimenContext();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create(request, context));
            Assert.Contains("WRAPPED_INNER_EXCEPTION", actualEx.Message);
            Assert.Contains("INNER_INNER_EXCEPTION", actualEx.Message);
        }
    }
}
