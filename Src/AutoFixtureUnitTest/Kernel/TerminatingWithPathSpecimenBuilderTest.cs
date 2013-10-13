using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TerminatingWithPathSpecimenBuilderTest
    {
        [Fact]
        public void SutIsISpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new TerminatingWithPathSpecimenBuilder(new DelegatingTracingBuilder());
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsISpecimenBuilderNode()
        {
            // Fixture setup
            // Exercise system
            var sut = new TerminatingWithPathSpecimenBuilder(new DelegatingTracingBuilder());
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
        public void TracerIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingTracingBuilder();
            // Exercise system
            var sut = new TerminatingWithPathSpecimenBuilder(expected);
            // Verify outcome
            Assert.Same(expected, sut.Tracer);
            // Teardown
        }

        [Fact]
        public void SutYieldsCorrectSequence()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var tracer = new DelegatingTracingBuilder(expected);
            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
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
            var tracer = new DelegatingTracingBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
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
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(tw.Tracer.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var tracer = new DelegatingTracingBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var tw = Assert.IsAssignableFrom<TerminatingWithPathSpecimenBuilder>(actual);
            Assert.Equal(expected, tw.Tracer.Builder);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var stubBuilder = new TracingBuilder(new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => expectedSpecimen
            });

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
            var mockBuilder = new TracingBuilder(new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => verified = expectedRequest == r && expectedContainer == c
            });

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
            var tracer = new DelegatingTracingBuilder();
            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
            // Exercise system and verify outcome
            Assert.Empty(sut.SpecimenRequests);
            // Teardown
        }

        [Fact]
        public void SpecimenRequestsRaisedFromTracerAreRecordedCorrectly()
        {
            // Fixture setup
            var tracer = new DelegatingTracingBuilder();
            var specimens = new[] { new object(), new object(), new object() };
            var requestEvents = specimens.Select((o, i) => new RequestTraceEventArgs(o, i)).ToList();
            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
            // Exercise system
            requestEvents.ForEach(tracer.RaiseSpecimenRequested);
            // Verify outcome
            Assert.True(specimens.SequenceEqual(sut.SpecimenRequests));
            // Teardown
        }

        [Fact]
        public void SpecimenRequestsAreEmptyWhenAllThatWereRequestedAreCreated()
        {
            // Fixture setup
            var tracer = new DelegatingTracingBuilder();
            var specimens = new[] { new object(), new object(), new object() };
            var requestEvents = specimens.Select(
                (o, i) => new RequestTraceEventArgs(o, i)).ToList();
            var createdEvents = specimens.Reverse().Select(
                (o, i) => new SpecimenCreatedEventArgs(o, null, i)).ToList();
            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
            // Exercise system
            requestEvents.ForEach(tracer.RaiseSpecimenRequested);
            createdEvents.ForEach(tracer.RaiseSpecimenCreated);
            // Verify outcome
            Assert.Empty(sut.SpecimenRequests);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenNoSpecimenIsReturnedFromTheDecoratedGraph()
        {
            // Fixture setup
            var requests = new[] {new object(), new object(), new object()};
            var tracer = new DelegatingTracingBuilder(new DelegatingSpecimenBuilder
            {
                // Returns NoSpecimen only on the last specimen request
                OnCreate = (r, c) => (r == requests[2]) ? new NoSpecimen() : new object(),
            });

            var sut = new TerminatingWithPathSpecimenBuilder(tracer);
            var container = new SpecimenContext(sut);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Create(requests[0], container));
            Assert.DoesNotThrow(() => sut.Create(requests[1], container));
            Assert.Throws<ObjectCreationException>(() => sut.Create(requests[2], container));
            // Teardown
        }
    }
}
