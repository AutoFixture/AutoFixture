using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TracingBuilderTest
    {
        [Fact]
        public void TestSpecificSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingTracingBuilder();
            // Verify outcome
            Assert.IsAssignableFrom<TracingBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingTracingBuilder();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecimenBuilderWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => 
                new DelegatingTracingBuilder(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new TracingBuilder(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void CreateWillPassThroughToDecoratedBuilder()
        {
            // Fixture setup
            object expectedSpecimen = Guid.NewGuid();
            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingTracingBuilder(decoratedBuilder);

            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            object result = sut.Create(expectedSpecimen, dummyContainer);

            // Verify outcome
            Assert.Equal(expectedSpecimen, result);

            // Teardown
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenRequested()
        {
            // Fixture setup
            var verified = false;
            var request = new object();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenRequested += (sender, e) => verified = e.Request == request && e.Depth == 1;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Event raised");
            // Teardown
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenRequestedInCompositeRequest()
        {
            // Fixture setup
            object requestedObject = "The request";
            object subRequest = "Some sub request";

            var spy = new List<RequestTraceEventArgs>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenRequested += (sender, e) => spy.Add(e);

            var container = new SpecimenContext(sut);
            // Exercise system
            sut.Create(requestedObject, container);
            // Verify outcome
            Assert.Equal(2, spy.Count);
            Assert.Equal(subRequest, spy[1].Request);
            Assert.Equal(2, spy[1].Depth);
            // Teardown
        }

        [Fact]
        public void CreateWillTrackCompositeRequests()
        {
            // Fixture setup
            object requestedObject = "The request";
            object subRequest = "Some sub request";
            var spy = new List<object>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenRequested += (sender, e) => spy.Add(e.Request);
            var container = new SpecimenContext(sut);
            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(2, spy.Count);
            Assert.Equal(subRequest, spy[1]);

            // Teardown
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenCreated()
        {
            // Fixture setup
            var request = new object();
            var specimen = new object();

            var verified = false;
            var sut = new DelegatingTracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => specimen });
            sut.SpecimenCreated += (sender, e) => verified = e.Request == request && e.Specimen == specimen && e.Depth == 1;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Event raised");
            // Teardown
        }

        [Fact]
        public void CreateWillTrackCreatedSpecimen()
        {
            // Fixture setup
            object tracked = null;
            object createdSpecimen = Guid.NewGuid();
            var container = new DelegatingSpecimenContext { OnResolve = r => createdSpecimen };
            var sut = new DelegatingTracingBuilder();

            // Exercise system
            object res = sut.Create(new object(), container);

            // Verify outcome
            Assert.Equal(res, tracked);

            // Teardown
        }

        [Fact]
        public void CreateWillCorrectRaiseSpecimenCreatedInCompositeRequest()
        {
            // Fixture setup
            object request = "The request";
            object subRequest = "Some sub request";
            var createdSpecimen = new object();

            var spy = new List<SpecimenCreatedEventArgs>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenCreated += (sender, e) => spy.Add(e);

            var container = new SpecimenContext(sut);
            // Exercise system
            sut.Create(request, container);
            // Verify outcome
            Assert.Equal(2, spy.Count);
            Assert.Equal(createdSpecimen, spy[0].Specimen);
            Assert.Equal(2, spy[0].Depth);
            Assert.Equal(createdSpecimen, spy[1].Specimen);
            Assert.Equal(1, spy[1].Depth);
            // Teardown
        }

        [Fact]
        public void CreateWillTrackCreatedSpecimensComposite()
        {
            // Fixture setup
            object requestedObject = "The request";
            object subRequest = "Some sub request";
            object createdSpecimen = Guid.NewGuid();
            var spy = new List<object>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenCreated += (sender, e) => spy.Add(e.Specimen);
            var container = new SpecimenContext(sut);
            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(2, spy.Count);
            Assert.Equal(createdSpecimen, spy[0]);
            Assert.Equal(createdSpecimen, spy[1]);

            // Teardown
        }

        [Fact]
        public void AssignNullFilterWillThrow()
        {
            // Fixture setup
            var sut = new DelegatingTracingBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Filter = null);
            // Teardown
        }

        [Fact]
        public void FilterIsProperWritableProperty()
        {
            // Fixture setup
            var sut = new DelegatingTracingBuilder();
            IRequestSpecification expectedFilter = new DelegatingRequestSpecification();
            // Exercise system
            sut.Filter = expectedFilter;
            IRequestSpecification result = sut.Filter;
            // Verify outcome
            Assert.Equal(expectedFilter, result);
            // Teardown
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenRequestForFilteredRequest()
        {
            // Fixture setup
            var eventRaised = false;

            var request = new object();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenRequested += (sender, e) => eventRaised = true;
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.False(eventRaised, "Event raised");
            // Teardown
        }

        [Fact]
        public void CreateWillNotTrackFilteredRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = new object();
            var sut = new DelegatingTracingBuilder();
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(requestedObject, dummyContainer);
            // Verify outcome
            Assert.Null(tracked);
            // Teardown
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenCreatedForFilteredRequest()
        {
            // Fixture setup
            var eventRaised = false;

            var request = new object();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenCreated += (sender, e) => eventRaised = true;
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.False(eventRaised, "Event raised");
            // Teardown
        }

        [Fact]
        public void CreateWillNotTrackFilteredSpecimen()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = new object();
            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingTracingBuilder(decoratedBuilder);
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(requestedObject, dummyContainer);
            // Verify outcome
            Assert.Null(tracked);
            // Teardown
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenRequestedForIgnoredType()
        {
            // Fixture setup
            var eventRaised = false;

            var request = Guid.NewGuid();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenRequested += (sender, e) => eventRaised = true;

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.False(eventRaised, "Event raised");
            // Teardown
        }

        [Fact]
        public void IgnoredTypeWillNotTrackRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = Guid.NewGuid();
            var sut = new DelegatingTracingBuilder();

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };

            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(requestedObject, dummyContainer);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }

        [Fact]
        public void DepthWillBeResetAfterDecoratedBuilderThrows()
        {
            // Fixture setup
            int createCallNumber = 0;
            int lastRequestDepth = 0;
            var firstRequest = Guid.NewGuid();
            var secondRequest = Guid.NewGuid();
            var dummyContainer = new DelegatingSpecimenContext();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) =>
            {
                createCallNumber++;
                if (createCallNumber == 1) throw new PrivateException();
                return c.Resolve(r);
            }};
            var sut = new DelegatingTracingBuilder(builder);
            sut.SpecimenRequested += (sender, e) => lastRequestDepth = e.Depth;

            // Exercise system and verify outcome
            Assert.Throws<PrivateException>(() => sut.Create(firstRequest, dummyContainer));
            Assert.Equal(1, lastRequestDepth);

            sut.Create(secondRequest, dummyContainer);
            Assert.Equal(1, lastRequestDepth);
        }

        class PrivateException : Exception { }

        [Fact]
        public void CreateWillNotRaiseSpecimenCreatedForIgnoredType()
        {
            // Fixture setup
            var eventRaised = false;

            var request = Guid.NewGuid();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenCreated += (sender, e) => eventRaised = true;

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.False(eventRaised, "Event raised");
            // Teardown
        }

        [Fact]
        public void IgnoredTypeWillNotTrackCreatedSpecimen()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = Guid.NewGuid();

            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingTracingBuilder(decoratedBuilder);

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };

            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            object res = sut.Create(requestedObject, dummyContainer);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }
    }
}