using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TerminatingWithPathSpecimenBuilderTest
    {
        [Fact]
        public void SutIsISpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new TerminatingWithPathSpecimenBuilder(new DelegatingSpecimenBuilder());
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsISpecimenBuilderNode()
        {
            // Fixture setup
            // Exercise system
            var sut = new TerminatingWithPathSpecimenBuilder(new DelegatingSpecimenBuilder());
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }
        
        [Fact]
        public void InitializeWithNullBuilderWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TerminatingWithPathSpecimenBuilder(null));
            // Teardown
        }

        [Fact]
        public void SpecimenBuilderIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new TerminatingWithPathSpecimenBuilder(expected);
            // Verify outcome
            Assert.Same(expected, sut.Builder);
            // Teardown
        }

        [Fact]
        public void SutYieldsCorrectSequence()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(expected);
            // Exercise system
            // Verify outcome
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, sut.Cast<object>().Single());
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var tw = Assert.IsAssignableFrom<TerminatingWithPathSpecimenBuilder>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(tw.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var tw = Assert.IsAssignableFrom<TerminatingWithPathSpecimenBuilder>(actual);
            Assert.Equal(expected, tw.Builder);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var stubBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expectedSpecimen
            };

            var sut = new TerminatingWithPathSpecimenBuilder(stubBuilder);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeDecoratedBuilderWithCorrectParameters()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var mockBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => verified = expectedRequest == r && expectedContainer == c
            };

            var sut = new TerminatingWithPathSpecimenBuilder(mockBuilder);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void SpecimenRequestsWillInitiallyBeEmpty()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Exercise system and verify outcome
            Assert.Empty(sut.SpecimenRequests);
            // Teardown
        }

        [Fact]
        public void SpecimenRequestsRaisedFromTracerAreRecordedCorrectly()
        {
            // Fixture setup
            var requests = new[] { new object(), new object(), new object() };
            var requestQueue = new Queue<object>(requests);
            var firstRequest = requestQueue.Dequeue();

            object[] capturedRequests = null;
            TerminatingWithPathSpecimenBuilder sut = null;
            
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                {
                    if(requestQueue.Count > 0) 
                        return c.Resolve(requestQueue.Dequeue());
                    
                    capturedRequests = sut.SpecimenRequests.ToArray();
                    return new object();
                }
            };
            
            sut = new TerminatingWithPathSpecimenBuilder(builder);
            // Cause sut to be executed recursively for multiple times.
            var context = new SpecimenContext(sut);
            
            // Exercise system
            sut.Create(firstRequest, context);
            
            // Verify outcome
            Assert.NotNull(capturedRequests);
            Assert.Equal(requests, capturedRequests);
            // Teardown
        }

        [Fact]
        public void SpecimenRequestsAreEmptyWhenAllThatWereRequestedAreCreated()
        {
            // Fixture setup
            var requests = new[] {new object(), new object(), new object()};
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

            // Exercise system
            sut.Create(firstRequest, context);

            // Verify outcome
            Assert.Empty(sut.SpecimenRequests);
            // Teardown
        }

        [Fact]
        public void SpecimenRequestsAreEmptyAfterThrowing()
        {
            // Fixture setup
            var requests = new[] {new object(), new object(), new object() };
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

            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() => sut.Create(firstRequest, container));

            Assert.Empty(sut.SpecimenRequests);
        }

        [Fact]
        public void CreateThrowsWhenNoSpecimenIsReturnedFromTheDecoratedGraph()
        {
            // Fixture setup
            var requests = new[] {new object(), new object(), new object()};
            var builder = new DelegatingSpecimenBuilder
            {
                // Returns NoSpecimen only on the last specimen request
                OnCreate = (r, c) => r == requests[2] ? new NoSpecimen() : new object()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var container = new SpecimenContext(sut);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Create(requests[0], container));
            Assert.DoesNotThrow(() => sut.Create(requests[1], container));
            Assert.Throws<ObjectCreationException>(() => sut.Create(requests[2], container));
            // Teardown
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
            var e = Assert.Throws<ObjectCreationException>(
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
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => 99
            };
            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r => 99
            };
            // Exercise system
            int[] specimens = Enumerable.Range(0, 1000)
                .AsParallel()
                    .WithDegreeOfParallelism(8)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x => (int)sut.Create(typeof(int), dummyContext))
                .ToArray();
            // Verify outcome
            Assert.Equal(1000, specimens.Length);
            Assert.True(specimens.All(s => s == 99));
            // Teardown
        }

        [Fact]
        public void ObjectCreationExceptionIsThrownIfCreationFails()
        {
            // Fixture setup
            var request = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw new Exception()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() => sut.Create(request, context));
            // Teardown
        }

        [Fact]
        public void ExceptionWithCorrectMessageIsThrownIfCreationFails()
        {
            // Fixture setup
            var request = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw new Exception()
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Exercise system and verify outcome
            var actualException = Assert.Throws<ObjectCreationException>(() => sut.Create(request, context));
            Assert.Contains("failed with exception", actualException.Message);
            // Teardown
        }

        [Fact]
        public void InnerBuilderExceptionIsWrappedByCreationException()
        {
            // Fixture setup
            var request = new object();
            var exception = new Exception();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw exception
            };

            var sut = new TerminatingWithPathSpecimenBuilder(builder);
            var context = new DelegatingSpecimenContext();

            // Exercise system and verify outcome
            var actualEx = Assert.Throws<ObjectCreationException>(() => sut.Create(request, context));
            Assert.Same(exception, actualEx.InnerException);
            // Teardown
        }

        [Fact]
        public void NestedExceptionIsWrappedForOneTimeOnly()
        {
            // Fixture setup
            var requests = new[] {new object(), new object(), new object()};
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

            // Exercise system and verify outcome
            var actualEx = Assert.Throws<ObjectCreationException>(() => sut.Create(firstRequest, context));
            Assert.IsAssignableFrom<InvalidOperationException>(actualEx.InnerException);
            // Teardown
        }
    }
}
