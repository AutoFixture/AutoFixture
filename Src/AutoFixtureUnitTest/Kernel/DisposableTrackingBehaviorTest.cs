using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class DisposableTrackingBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Arrange
            // Act
            var sut = new DisposableTrackingBehavior();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
        }

        [Fact]
        public void TransformReturnsCorrectSpecimenBuilderType()
        {
            // Arrange
            var sut = new DisposableTrackingBehavior();
            // Act
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<DisposableTracker>(result);
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Arrange
            var sut = new DisposableTrackingBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Act
            var result = sut.Transform(expectedBuilder);
            // Assert
            var tracker = Assert.IsAssignableFrom<DisposableTracker>(result);
            Assert.Equal(expectedBuilder, tracker.Builder);
        }

        [Fact]
        public void TrackersIsInstance()
        {
            // Arrange
            var sut = new DisposableTrackingBehavior();
            // Act
            IEnumerable<DisposableTracker> result = sut.Trackers;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void TransformAddsItemsToTrackers()
        {
            // Arrange
            var sut = new DisposableTrackingBehavior();
            // Act
            var trackers = new[]
            {
                sut.Transform(new DelegatingSpecimenBuilder()),
                sut.Transform(new DelegatingSpecimenBuilder()),
                sut.Transform(new DelegatingSpecimenBuilder())
            };
            // Assert
            Assert.True(trackers.All(sb => sut.Trackers.Any(dt => sb == dt)));
        }

        [Fact]
        public void SutIsDisposable()
        {
            // Arrange
            // Act
            var sut = new DisposableTrackingBehavior();
            // Assert
            Assert.IsAssignableFrom<IDisposable>(sut);
        }

        [Fact]
        public void DisposeDisposesTrackers()
        {
            // Arrange
            var sut = new DisposableTrackingBehavior();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new DisposableSpy() };
            var trackers = new[]
            {
                (DisposableTracker)sut.Transform(builder),
                (DisposableTracker)sut.Transform(builder),
                (DisposableTracker)sut.Transform(builder)
            };

            trackers.ToList().ForEach(dt => Enumerable.Repeat(new object(), 3).Select(r => dt.Create(r, new DelegatingSpecimenContext())).ToList());
            // Act
            sut.Dispose();
            // Assert
            Assert.True((from dt in trackers
                         from d in dt.Disposables
                         select d).Cast<DisposableSpy>().All(ds => ds.Disposed));
        }

        [Fact]
        public void DisposeClearsTrackers()
        {
            // Arrange
            var sut = new DisposableTrackingBehavior();
            sut.Transform(new DelegatingSpecimenBuilder());
            sut.Transform(new DelegatingSpecimenBuilder());
            sut.Transform(new DelegatingSpecimenBuilder());
            // Act
            sut.Dispose();
            // Assert
            Assert.Empty(sut.Trackers);
        }
    }
}
