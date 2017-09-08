using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DisposableTrackerTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DisposableTracker(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsNode()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DisposableTracker(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DisposableTracker(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void SutYieldsInjectedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(expected);
            // Exercise system
            // Verify outcome
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
            // Teardown
        }

        [Fact]
        public void CreateReturnsResultFromDecoratedBuilder()
        {
            // Fixture setup
            var request = new object();
            var ctx = new DelegatingSpecimenContext();
            var expectedResult = new object();

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => (r == request) && (c == ctx) ? expectedResult : new NoSpecimen()
            };

            var sut = new DisposableTracker(builder);
            // Exercise system
            var result = sut.Create(request, ctx);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummyBuilder);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var dt = Assert.IsAssignableFrom<DisposableTracker>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(dt.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummyBuilder);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var dt = Assert.IsAssignableFrom<DisposableTracker>(actual);
            Assert.Equal(expected, dt.Builder);
            // Teardown
        }

        [Fact]
        public void DisposablesIsInstance()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummyBuilder);
            // Exercise system
            IEnumerable<IDisposable> result = sut.Disposables;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void DecoratedDisposableResultIsAddedToDisposables()
        {
            // Fixture setup
            var disposable = new DisposableSpy();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => disposable };
            var sut = new DisposableTracker(builder);
            // Exercise system
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Contains(disposable, sut.Disposables);
            // Teardown
        }

        [Fact]
        public void DecoratedDisposableResultIsOnlyAddedToDisposablesOnce()
        {
            // Fixture setup
            var disposable = new DisposableSpy();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => disposable };
            var sut = new DisposableTracker(builder);
            // Exercise system
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContext);
            sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(1, sut.Disposables.Count(d => d == disposable));
            // Teardown
        }

        [Fact]
        public void MultipleDecoratedDisposablesAreAddedToDisposables()
        {
            // Fixture setup
            var disposables = Enumerable.Repeat<Func<DisposableSpy>>(() => new DisposableSpy(), 3).Select(f => f()).ToList();
            var q = new Queue<DisposableSpy>(disposables);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => q.Dequeue() };

            var sut = new DisposableTracker(builder);
            // Exercise system
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            disposables.ForEach(d => sut.Create(dummyRequest, dummyContext));
            // Verify outcome
            Assert.True(disposables.All(ds => sut.Disposables.Any(d => d == ds)));
            // Teardown
        }

        [Fact]
        public void SutIsDisposable()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DisposableTracker(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<IDisposable>(sut);
            // Teardown
        }

        [Fact]
        public void DisposeDisposesAllDisposables()
        {
            // Fixture setup
            var disposables = Enumerable.Repeat<Func<DisposableSpy>>(() => new DisposableSpy(), 3).Select(f => f()).ToList();
            var q = new Queue<DisposableSpy>(disposables);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => q.Dequeue() };

            var sut = new DisposableTracker(builder);

            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            disposables.ForEach(d => sut.Create(dummyRequest, dummyContext));
            // Exercise system
            sut.Dispose();
            // Verify outcome
            Assert.True(sut.Disposables.Cast<DisposableSpy>().All(ds => ds.Disposed));
            // Teardown
        }

        [Fact]
        public void DisposeRemovesAllDisposables()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new DisposableSpy() };
            var sut = new DisposableTracker(builder);

            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();

            sut.Create(dummyRequest, dummyContext);
            sut.Create(dummyRequest, dummyContext);
            sut.Create(dummyRequest, dummyContext);
            // Exercise system
            sut.Dispose();
            // Verify outcome
            Assert.Empty(sut.Disposables);
            // Teardown
        }

        [Fact]
        public void ComposeAddsReturnedObjectToDisposables()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new DisposableTracker(dummy);
            // Exercise system
            var dummies = new ISpecimenBuilder[0];
            var actual = sut.Compose(dummies);
            // Verify outcome
            Assert.True(
                sut.Disposables.Any(actual.Equals),
                "Returned value not added to disposables.");
            // Teardown
        }
    }
}
