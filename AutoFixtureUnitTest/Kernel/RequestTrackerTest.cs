namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Kernel;
    using Xunit;

    public class RequestTrackerTest
    {
        [Fact]
        public void CreateWillPassThroughToContainer()
        {
            // Fixture setup
            object someSpecimen = Guid.NewGuid();
            var container = new DelegatingSpecimenContainer { OnCreate = r => someSpecimen };
            var sut = new DelegatingRequestTracker();

            // Exercise system
            object res = sut.Create(new object(), container);

            // Verify outcome
            Assert.Equal(someSpecimen, res);

            // Teardown
        }

        [Fact]
        public void CreateWillAllowRestOfCompositeBuilderPipelineToRun()
        {
            // Fixture setup
            object someSpecimen = Guid.NewGuid();
            var sut = new DelegatingRequestTracker();
            var nextBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => someSpecimen };
            var compBuilder = new CompositeSpecimenBuilder(sut, nextBuilder);

            // Exercise system
            object res = sut.Create(new object(), new DefaultSpecimenContainer(compBuilder));

            // Verify outcome
            Assert.Equal(someSpecimen, res);

            // Teardown
        }

        [Fact]
        public void CreateWillTrackIngoingRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = new object();
            var container = new DelegatingSpecimenContainer();
            var sut = new DelegatingRequestTracker { OnTrackRequest = r => tracked = r };

            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(requestedObject, tracked);

            // Teardown
        }

        [Fact]
        public void CreateWillTrackRequestsInCompositeBuilderPipeline()
        {
            // Fixture setup
            object requestedObject = "The request";
            object subRequest = "Some sub request";
            var tracked = new List<object>();
            var sut = new DelegatingRequestTracker { OnTrackRequest = tracked.Add };
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(sut, builder2, builder3);

            // Exercise system
            sut.Create(requestedObject, new DefaultSpecimenContainer(compBuilder));

            // Verify outcome
            Assert.Equal(2, tracked.Count);
            Assert.Equal(subRequest, tracked[1]);

            // Teardown
        }

        [Fact]
        public void CreateWillTrackCreatedSpecimen()
        {
            // Fixture setup
            object tracked = null;
            object createdSpecimen = Guid.NewGuid();
            var container = new DelegatingSpecimenContainer { OnCreate = r => createdSpecimen };
            var sut = new DelegatingRequestTracker { OnTrackCreatedSpecimen = r => tracked = r };

            // Exercise system
            object res = sut.Create(new object(), container);

            // Verify outcome
            Assert.Equal(res, tracked);

            // Teardown
        }

        [Fact]
        public void CreateWillTrackCreatedSpecimensInCompositeBuilderPipeline()
        {
            // Fixture setup
            object requestedObject = "The request";
            object subRequest = "Some sub request";
            object createdSpecimen = Guid.NewGuid();
            var tracked = new List<object>();
            var sut = new DelegatingRequestTracker { OnTrackCreatedSpecimen = tracked.Add };
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(sut, builder2, builder3);

            // Exercise system
            sut.Create(requestedObject, new DefaultSpecimenContainer(compBuilder));

            // Verify outcome
            Assert.Equal(2, tracked.Count);
            Assert.Equal(createdSpecimen, tracked[0]);
            Assert.Equal(createdSpecimen, tracked[1]);

            // Teardown
        }

        [Fact]
        public void IgnoredTypeWillNotTrackRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = Guid.NewGuid();
            var container = new DelegatingSpecimenContainer();
            var sut = new DelegatingRequestTracker { OnTrackRequest = r => tracked = r };
            sut.IgnoredTypes.Add(typeof(Guid));

            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }

        [Fact]
        public void IgnoredTypeWillNotTrackCreatedSpecimen()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = Guid.NewGuid();
            var container = new DelegatingSpecimenContainer();
            var sut = new DelegatingRequestTracker { OnTrackCreatedSpecimen = r => tracked = r };
            sut.IgnoredTypes.Add(typeof(Guid));

            // Exercise system
            object res = sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }

        private class DelegatingRequestTracker : RequestTracker
        {
            public DelegatingRequestTracker()
            {
                this.OnTrackRequest = r => { };
                this.OnTrackCreatedSpecimen = r => { };
            }

            protected override void TrackRequest(object request)
            {
                OnTrackRequest(request);
            }

            protected override void TrackCreatedSpecimen(object specimen)
            {
                OnTrackCreatedSpecimen(specimen);
            }

            internal Action<object> OnTrackRequest { get; set; }
            internal Action<object> OnTrackCreatedSpecimen { get; set; }
        }
    }
}