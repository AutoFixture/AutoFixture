namespace Ploeh.AutoFixtureUnitTest.Kernel
{
	using System;
	using AutoFixture.Kernel;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class RequestTrackerTest
	{
		[TestMethod]
		public void CreateWillPassThroughToContainer()
		{
			// Fixture setup
			var container = new DelegatingSpecimenContainer { OnCreate = r => true };
			var sut = new RequestTrackerImplForTest();

			// Exercise system
			object res = sut.Create(new object(), container);

			// Verify outcome
			Assert.IsInstanceOfType(res, typeof(bool), "Expected container's create to be invoked.");

			// Teardown
		}

		private class RequestTrackerImplForTest : RequestTracker
		{
			protected override void TrackRequest(object request)
			{
				
			}

			protected override void TrackCreatedSpecimen(object specimen)
			{
				
			}
		}
	}
}