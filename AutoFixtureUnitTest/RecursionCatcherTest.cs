namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using AutoFixture;
    using AutoFixture.Kernel;
    using Kernel;
    using Xunit;

    public class RecursionCatcherTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RecursionCatcher(new InterceptingBuilder(new DelegatingSpecimenBuilder()));
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void TrackRequestWillNotTriggerHandlingOnFirstRequest()
        {
            // Fixture setup
            var sut = new RecursionCatcher(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            sut.RecursionRequestInterceptor = obj => handlingTriggered = true;

            // Exercise system
            sut.Create(Guid.NewGuid(), new DelegatingSpecimenContainer());

            // Verify outcome
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void TrackRequestWillNotTriggerHandlingOnSubsequentSimilarRequests()
        {
            // Fixture setup
            var sut = new RecursionCatcher(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            object request = Guid.NewGuid();
            sut.RecursionRequestInterceptor = obj => handlingTriggered = true;

            // Exercise system
            sut.Create(request, new DelegatingSpecimenContainer());
            sut.Create(request, new DelegatingSpecimenContainer());

            // Verify outcome
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void TrackRequestWillTriggerHandlingOnRecursiveRequests()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new RecursionCatcher(builder);
            bool handlingTriggered = false;
            sut.RecursionRequestInterceptor = obj => handlingTriggered = true;
            var container = new DelegatingSpecimenContainer();
            container.OnResolve = (r) => sut.Create(r, container);

            // Exercise system
            sut.Create(Guid.NewGuid(), container);

            Assert.True(handlingTriggered);
        }
    }
}