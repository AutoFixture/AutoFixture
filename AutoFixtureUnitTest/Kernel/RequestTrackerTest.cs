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
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DelegatingRequestTracker(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<RequestTracker>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DelegatingRequestTracker(dummyBuilder);
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

        //[Fact]
        //public void CreateWillAllowRestOfCompositeBuilderPipelineToRun()
        //{
        //    // Fixture setup
        //    object someSpecimen = Guid.NewGuid();
        //    var sut = new DelegatingRequestTracker();
        //    var nextBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => someSpecimen };
        //    var compBuilder = new CompositeSpecimenBuilder(sut, nextBuilder);

        //    // Exercise system
        //    object res = sut.Create(new object(), new DefaultSpecimenContainer(compBuilder));

        //    // Verify outcome
        //    Assert.Equal(someSpecimen, res);

        //    // Teardown
        //}

        [Fact]
        public void CreateWillTrackIngoingRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = new object();
            var container = new DelegatingSpecimenContainer();
            var sut = new DelegatingRequestTracker(new DelegatingSpecimenBuilder()) { OnTrackRequest = r => tracked = r };

            // Exercise system
            sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Equal(requestedObject, tracked);

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
        public void CreateWillTrackCreatedSpecimen()
        {
            // Fixture setup
            object tracked = null;
            object createdSpecimen = Guid.NewGuid();
            var container = new DelegatingSpecimenContainer { OnCreate = r => createdSpecimen };
            var sut = new DelegatingRequestTracker(new DelegatingSpecimenBuilder()) { OnTrackCreatedSpecimen = r => tracked = r };

            // Exercise system
            object res = sut.Create(new object(), container);

            // Verify outcome
            Assert.Equal(res, tracked);

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
        public void IgnoredTypeWillNotTrackRequest()
        {
            // Fixture setup
            object tracked = null;
            object requestedObject = Guid.NewGuid();
            var container = new DelegatingSpecimenContainer();
            var sut = new DelegatingRequestTracker(new DelegatingSpecimenBuilder()) { OnTrackRequest = r => tracked = r };
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
            var sut = new DelegatingRequestTracker(new DelegatingSpecimenBuilder()) { OnTrackCreatedSpecimen = r => tracked = r };
            sut.IgnoredTypes.Add(typeof(Guid));

            // Exercise system
            object res = sut.Create(requestedObject, container);

            // Verify outcome
            Assert.Null(tracked);

            // Teardown
        }

        private class DelegatingRequestTracker : RequestTracker
        {
            public DelegatingRequestTracker(ISpecimenBuilder builder)
                : base(builder)
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