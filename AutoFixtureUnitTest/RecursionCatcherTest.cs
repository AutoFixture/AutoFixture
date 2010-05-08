namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using AutoFixture.Kernel;
    using Kernel;
    using Xunit;

    public class RecursionCatcherTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DelegatingRecursionCatcher(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new DelegatingRecursionCatcher(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEqualityComparerThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new DelegatingRecursionCatcher(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void CreateWillUseEqualityComparer()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            bool comparerUsed = false;
            var comparer = new DelegatingEqualityComparer { OnEquals = (x, y) => comparerUsed = true };
            var sut = new DelegatingRecursionCatcher(builder, comparer);
            sut.OnHandleRecursiveRequest = (obj) => { return null; };
            var container = new DelegatingSpecimenContainer();
            container.OnResolve = (r) => sut.Create(r, container);

            // Exercise system
            sut.Create(Guid.NewGuid(), container);

            // Verify outcome
            Assert.True(comparerUsed);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnFirstRequest()
        {
            // Fixture setup
            var sut = new DelegatingRecursionCatcher(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;

            // Exercise system
            sut.Create(Guid.NewGuid(), new DelegatingSpecimenContainer());

            // Verify outcome
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnSubsequentSimilarRequests()
        {
            // Fixture setup
            var sut = new DelegatingRecursionCatcher(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            object request = Guid.NewGuid();
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;

            // Exercise system
            sut.Create(request, new DelegatingSpecimenContainer());
            sut.Create(request, new DelegatingSpecimenContainer());

            // Verify outcome
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void CreateWillTriggerHandlingOnRecursiveRequests()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new DelegatingRecursionCatcher(builder);
            bool handlingTriggered = false;
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;
            var container = new DelegatingSpecimenContainer();
            container.OnResolve = (r) => sut.Create(r, container);

            // Exercise system
            sut.Create(Guid.NewGuid(), container);

            Assert.True(handlingTriggered);
        }
    }
}