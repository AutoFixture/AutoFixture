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
            var sut = new RecursionCatcher(new InterceptingBuilder(new DelegatingSpecimenBuilder()));
            bool handlingTriggered = false;
            sut.RecursionRequestInterceptor = obj => handlingTriggered = true;

            sut.Create(Guid.NewGuid(), new DelegatingSpecimenContainer());

            Assert.False(handlingTriggered);
        }

		[Fact]
		public void TrackRequestWillNotTriggerHandlingOnSubsequentSimilarRequests()
		{
            var sut = new RecursionCatcher(new InterceptingBuilder(new DelegatingSpecimenBuilder()));
		    bool handlingTriggered = false;
            object request = Guid.NewGuid();
            sut.RecursionRequestInterceptor = obj => handlingTriggered = true;

            sut.Create(request, new DelegatingSpecimenContainer());
            sut.Create(request, new DelegatingSpecimenContainer());

		    Assert.False(handlingTriggered);
		}
	}
}