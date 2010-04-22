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
		public void TrackRequestWillTriggerHandlingOnFirstRecurrenceOfRequest()
		{
			var sut = new RecursionCatcherForTest();
		    bool handlingTriggered = false;
		    sut.OnGetRecursionBreakInstance = obj => handlingTriggered = true;

			sut.InvokeTrackRequest("Dip");
			sut.InvokeTrackRequest("Dip");

		    Assert.True(handlingTriggered);
		}

		private class RecursionCatcherForTest : RecursionCatcher
		{
            public RecursionCatcherForTest() : this(new DelegatingSpecimenBuilder())
		    {
		    }

		    public RecursionCatcherForTest(ISpecimenBuilder builder) : base(builder)
		    {
		    }

		    public void InvokeTrackRequest(object request)
			{
				this.TrackRequest(request);
			}

			internal Func<object, object> OnGetRecursionBreakInstance;

			protected override object GetRecursionBreakInstance(object request)
			{
			    return OnGetRecursionBreakInstance(request);
			}
		}
	}
}