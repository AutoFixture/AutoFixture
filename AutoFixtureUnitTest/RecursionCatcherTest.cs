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
        public void TestSpecificSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingRecursionCatcher();
            // Verify outcome
            Assert.IsAssignableFrom<RecursionCatcher>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsRequestTracker()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingRecursionCatcher();
            // Verify outcome
            Assert.IsAssignableFrom<RequestTracker>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingRecursionCatcher();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void TrackRequestWillNotTriggerHandlingOnFirstRequest()
        {
            var sut = new DelegatingRecursionCatcher();
            bool handlingTriggered = false;
            sut.OnGetRecursionBreakSpecimen = obj => handlingTriggered = true;

            sut.InvokeTrackRequest(Guid.NewGuid());

            Assert.False(handlingTriggered);
        }

		[Fact]
		public void TrackRequestWillTriggerHandlingOnFirstRecurrenceOfRequest()
		{
			var sut = new DelegatingRecursionCatcher();
		    bool handlingTriggered = false;
            object request = Guid.NewGuid();
		    sut.OnGetRecursionBreakSpecimen = obj => handlingTriggered = true;

			sut.InvokeTrackRequest(request);
			sut.InvokeTrackRequest(request);

		    Assert.True(handlingTriggered);
		}

		private class DelegatingRecursionCatcher : RecursionCatcher
		{
            public DelegatingRecursionCatcher() : this(new DelegatingSpecimenBuilder())
		    {
		    }

		    public DelegatingRecursionCatcher(ISpecimenBuilder builder) : base(new InterceptingBuilder(builder))
		    {
		    }

		    public void InvokeTrackRequest(object request)
			{
				this.TrackRequest(request);
			}

			internal Func<object, object> OnGetRecursionBreakSpecimen;

			protected override object GetRecursionBreakSpecimen(object request)
			{
			    return OnGetRecursionBreakSpecimen(request);
			}
		}
	}
}