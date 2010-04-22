namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Kernel;
    using Xunit;

    public class RequestTrackerTest
    {
        [Fact]
        public void TestSpecificSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingRequestTracker();
            // Verify outcome
            Assert.IsAssignableFrom<RequestTracker>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingRequestTracker();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullSpecimenBuilderWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new DelegatingRequestTracker(null));
            // Teardown
        }

        [Fact]
        public void CreateWillPassThroughToDecoratedBuilder()
        {
            // Fixture setup
            object expectedSpecimen = Guid.NewGuid();
            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingRequestTracker(decoratedBuilder);

            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new DelegatingRequestTracker();
            sut.SpecimenRequested += (object sender, SpecimenTraceEventArgs e) => verified = e.Request == request && e.Depth == 1;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Event raised");
            // Teardown
        }

        [Fact]
        public void CreateWillTrackIngoingRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = new object();
            var container = new DelegatingSpecimenContainer();
            var sut = new DelegatingRequestTracker() { OnTrackRequest = r => tracked = r };

            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(requestedObject, tracked);

            // Teardown
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenRequestedInCompositeRequest()
        {
            // Fixture setup
            object requestedObject = "The request";
            object subRequest = "Some sub request";

            var spy = new List<SpecimenTraceEventArgs>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingRequestTracker(compBuilder);
            sut.SpecimenRequested += (object sender, SpecimenTraceEventArgs e) => spy.Add(e);

            var container = new DefaultSpecimenContainer(sut);
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
            var tracked = new List<object>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingRequestTracker(compBuilder) { OnTrackRequest = tracked.Add };
            var container = new DefaultSpecimenContainer(sut);
            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(2, tracked.Count);
            Assert.Equal(subRequest, tracked[1]);

            // Teardown
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenCreated()
        {
            // Fixture setup
            var request = new object();
            var specimen = new object();

            var verified = false;
            var sut = new DelegatingRequestTracker(new DelegatingSpecimenBuilder { OnCreate = (r, c) => specimen });
            sut.SpecimenCreated += (object sender, SpecimenCreatedEventArgs e) => verified = e.Request == request && e.Specimen == specimen && e.Depth == 1;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var container = new DelegatingSpecimenContainer { OnCreate = r => createdSpecimen };
            var sut = new DelegatingRequestTracker() { OnTrackCreatedSpecimen = r => tracked = r };

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
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request ? c.Create(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingRequestTracker(compBuilder);
            sut.SpecimenCreated += (object sender, SpecimenCreatedEventArgs e) => spy.Add(e);

            var container = new DefaultSpecimenContainer(sut);
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
            var tracked = new List<object>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingRequestTracker(compBuilder) { OnTrackCreatedSpecimen = tracked.Add };
            var container = new DefaultSpecimenContainer(sut);
            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(2, tracked.Count);
            Assert.Equal(createdSpecimen, tracked[0]);
            Assert.Equal(createdSpecimen, tracked[1]);

            // Teardown
        }

        [Fact]
        public void AssignNullFilterWillThrow()
        {
            // Fixture setup
            var sut = new DelegatingRequestTracker();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TrackSpecification = null);
            // Teardown
        }

        [Fact]
        public void FilterIsProperWritableProperty()
        {
            // Fixture setup
            var sut = new DelegatingRequestTracker();
            Func<object, bool> expectedFilter = obj => true;
            // Exercise system
            sut.TrackSpecification = expectedFilter;
            Func<object, bool> result = sut.TrackSpecification;
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
            var sut = new DelegatingRequestTracker();
            sut.SpecimenRequested += (object sender, SpecimenTraceEventArgs e) => eventRaised = true;
            sut.TrackSpecification = r => false;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new DelegatingRequestTracker() { OnTrackRequest = r => tracked = r };
            sut.TrackSpecification = r => false;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(requestedObject, dummyContainer);
            // Verify outcome
            Assert.Null(tracked);
            // Teardown
        }

        [Fact]
        public void CreateWillNotRaiseSpecimeCreatedForFilteredRequest()
        {
            // Fixture setup
            var eventRaised = false;

            var request = new object();
            var sut = new DelegatingRequestTracker();
            sut.SpecimenCreated += (object sender, SpecimenCreatedEventArgs e) => eventRaised = true;
            sut.TrackSpecification = r => false;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new DelegatingRequestTracker(decoratedBuilder) { OnTrackCreatedSpecimen = s => tracked = s };
            sut.TrackSpecification = r => false;
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new DelegatingRequestTracker();
            sut.SpecimenRequested += (object sender, SpecimenTraceEventArgs e) => eventRaised = true;

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.TrackSpecification = r => !ignoredTypes.Contains(r.GetType());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new DelegatingRequestTracker() { OnTrackRequest = r => tracked = r };

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.TrackSpecification = r => !ignoredTypes.Contains(r.GetType());

            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(requestedObject, dummyContainer);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenCreatedForIgnoredType()
        {
            // Fixture setup
            var eventRaised = false;

            var request = Guid.NewGuid();
            var sut = new DelegatingRequestTracker();
            sut.SpecimenCreated += (object sender, SpecimenCreatedEventArgs e) => eventRaised = true;

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.TrackSpecification = r => !ignoredTypes.Contains(r.GetType());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new DelegatingRequestTracker(decoratedBuilder) { OnTrackCreatedSpecimen = r => tracked = r };

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.TrackSpecification = r => !ignoredTypes.Contains(r.GetType());

            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            object res = sut.Create(requestedObject, dummyContainer);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }

        private class DelegatingRequestTracker : RequestTracker
        {
            public DelegatingRequestTracker()
                : this(new DelegatingSpecimenBuilder())
            {
            }

            public DelegatingRequestTracker(ISpecimenBuilder builder)
                : base(builder)
            {
                this.OnTrackRequest = r => { };
                this.OnTrackCreatedSpecimen = r => { };
            }

            protected override void TrackRequest(object request)
            {
                this.OnTrackRequest(request);
            }

            protected override void TrackCreatedSpecimen(object specimen)
            {
                this.OnTrackCreatedSpecimen(specimen);
            }

            internal Action<object> OnTrackRequest { get; set; }
            internal Action<object> OnTrackCreatedSpecimen { get; set; }
        }
    }
}