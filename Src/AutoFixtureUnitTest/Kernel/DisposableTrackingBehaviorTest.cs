using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DisposableTrackingBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new DisposableTrackingBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectSpecimenBuilderType()
        {
            // Fixture setup
            var sut = new DisposableTrackingBehavior();
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<DisposableTracker>(result);
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new DisposableTrackingBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(expectedBuilder);
            // Verify outcome
            var tracker = Assert.IsAssignableFrom<DisposableTracker>(result);
            Assert.Equal(expectedBuilder, tracker.Builder);
            // Teardown
        }

        [Fact]
        public void TrackersIsInstance()
        {
            // Fixture setup
            var sut = new DisposableTrackingBehavior();
            // Exercise system
            IEnumerable<DisposableTracker> result = sut.Trackers;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void TransformAddsItemsToTrackers()
        {
            // Fixture setup
            var sut = new DisposableTrackingBehavior();
            // Exercise system
            var trackers = new[]
            {
                sut.Transform(new DelegatingSpecimenBuilder()),
                sut.Transform(new DelegatingSpecimenBuilder()),
                sut.Transform(new DelegatingSpecimenBuilder())
            };
            // Verify outcome
            Assert.True(trackers.All(sb => sut.Trackers.Any(dt => sb == dt)));
            // Teardown
        }

        [Fact]
        public void SutIsDisposable()
        {
            // Fixture setup
            // Exercise system
            var sut = new DisposableTrackingBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<IDisposable>(sut);
            // Teardown
        }

        [Fact]
        public void DisposeDisposesTrackers()
        {
            // Fixture setup
            var sut = new DisposableTrackingBehavior();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new DisposableSpy() };
            var trackers = new[]
            {
                (DisposableTracker)sut.Transform(builder),
                (DisposableTracker)sut.Transform(builder),
                (DisposableTracker)sut.Transform(builder)
            };

            trackers.ToList().ForEach(dt => Enumerable.Repeat(new object(), 3).Select(r => dt.Create(r, new DelegatingSpecimenContext())).ToList());
            // Exercise system
            sut.Dispose();
            // Verify outcome
            Assert.True((from dt in trackers
                         from d in dt.Disposables
                         select d).Cast<DisposableSpy>().All(ds => ds.Disposed));
            // Teardown
        }

        [Fact]
        public void DisposeClearsTrackers()
        {
            // Fixture setup
            var sut = new DisposableTrackingBehavior();
            sut.Transform(new DelegatingSpecimenBuilder());
            sut.Transform(new DelegatingSpecimenBuilder());
            sut.Transform(new DelegatingSpecimenBuilder());
            // Exercise system
            sut.Dispose();
            // Verify outcome
            Assert.Empty(sut.Trackers);
            // Teardown
        }
    }
}
