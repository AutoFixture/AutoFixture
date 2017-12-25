using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class DisposableTrackerTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new DisposableTracker(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void SutIsNode()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new DisposableTracker(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new DisposableTracker(null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(expectedBuilder);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void SutYieldsInjectedBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(expected);
            // Act
            // Assert
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
        }

        [Fact]
        public void CreateReturnsResultFromDecoratedBuilder()
        {
            // Arrange
            var request = new object();
            var ctx = new DelegatingSpecimenContext();
            var expectedResult = new object();

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => (r == request) && (c == ctx) ? expectedResult : new NoSpecimen()
            };

            var sut = new DisposableTracker(builder);
            // Act
            var result = sut.Create(request, ctx);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummyBuilder);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var dt = Assert.IsAssignableFrom<DisposableTracker>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(dt.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummyBuilder);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var dt = Assert.IsAssignableFrom<DisposableTracker>(actual);
            Assert.Equal(expected, dt.Builder);
        }

        [Fact]
        public void DisposablesIsInstance()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummyBuilder);
            // Act
            IEnumerable<IDisposable> result = sut.Disposables;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DecoratedDisposableResultIsAddedToDisposables()
        {
            // Arrange
            var disposable = new DisposableSpy();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => disposable };
            var sut = new DisposableTracker(builder);
            // Act
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContext);
            // Assert
            Assert.Contains(disposable, sut.Disposables);
        }

        [Fact]
        public void DecoratedDisposableResultIsOnlyAddedToDisposablesOnce()
        {
            // Arrange
            var disposable = new DisposableSpy();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => disposable };
            var sut = new DisposableTracker(builder);
            // Act
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContext);
            sut.Create(dummyRequest, dummyContext);
            // Assert
            Assert.Equal(1, sut.Disposables.Count(d => d == disposable));
        }

        [Fact]
        public void MultipleDecoratedDisposablesAreAddedToDisposables()
        {
            // Arrange
            var disposables = Enumerable.Repeat<Func<DisposableSpy>>(() => new DisposableSpy(), 3).Select(f => f()).ToList();
            var q = new Queue<DisposableSpy>(disposables);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => q.Dequeue() };

            var sut = new DisposableTracker(builder);
            // Act
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            disposables.ForEach(d => sut.Create(dummyRequest, dummyContext));
            // Assert
            Assert.True(disposables.All(ds => sut.Disposables.Any(d => d == ds)));
        }

        [Fact]
        public void SutIsDisposable()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new DisposableTracker(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<IDisposable>(sut);
        }

        [Fact]
        public void DisposeDisposesAllDisposables()
        {
            // Arrange
            var disposables = Enumerable.Repeat<Func<DisposableSpy>>(() => new DisposableSpy(), 3).Select(f => f()).ToList();
            var q = new Queue<DisposableSpy>(disposables);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => q.Dequeue() };

            var sut = new DisposableTracker(builder);

            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            disposables.ForEach(d => sut.Create(dummyRequest, dummyContext));
            // Act
            sut.Dispose();
            // Assert
            Assert.True(sut.Disposables.Cast<DisposableSpy>().All(ds => ds.Disposed));
        }

        [Fact]
        public void DisposeRemovesAllDisposables()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new DisposableSpy() };
            var sut = new DisposableTracker(builder);

            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();

            sut.Create(dummyRequest, dummyContext);
            sut.Create(dummyRequest, dummyContext);
            sut.Create(dummyRequest, dummyContext);
            // Act
            sut.Dispose();
            // Assert
            Assert.Empty(sut.Disposables);
        }

        [Fact]
        public void ComposeAddsReturnedObjectToDisposables()
        {
            // Arrange
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummy);
            // Act
            var dummies = new ISpecimenBuilder[0];
            var actual = sut.Compose(dummies);
            // Assert
            Assert.True(
                sut.Disposables.Any(actual.Equals),
                "Returned value not added to disposables.");
        }
    }
}
