namespace Ploeh.AutoFixtureUnitTest.Kernel
{
	using System;
	using System.Collections.Generic;
	using AutoFixture.Kernel;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class RequestTrackerTest
	{
		[TestMethod]
		public void CreateWillPassThroughToContainer()
		{
			// Fixture setup
			object someSpecimen = Guid.NewGuid();
			var container = new DelegatingSpecimenContainer { OnCreate = r => someSpecimen };
			var sut = new DelegatingRequestTracker();

			// Exercise system
			object res = sut.Create(new object(), container);

			// Verify outcome
			Assert.AreEqual(someSpecimen, res, "Expected container's create to be invoked.");

			// Teardown
		}

		[TestMethod]
		public void CreateWillAllowRestOfCompositeBuilderPipelineToRun()
		{
			// Fixture setup
			object someSpecimen = Guid.NewGuid();
			var sut = new DelegatingRequestTracker();
			var nextBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => someSpecimen };
			var compBuilder = new CompositeSpecimenBuilder(sut, nextBuilder);

			// Exercise system
			object res = sut.Create(new object(), new DefaultSpecimenContainer(compBuilder));

			// Verify outcome
			Assert.AreEqual(someSpecimen, res, "Expected the next builder in composite container's pipeline to have Create invoked.");

			// Teardown
		}

		[TestMethod]
		public void CreateWillTrackIngoingRequest()
		{
			// Fixture setup
			object tracked = null;
			object requestedObject = new object();
			var container = new DelegatingSpecimenContainer();
			var sut = new DelegatingRequestTracker { OnTrackRequest = r => tracked = r };

			// Exercise system
			sut.Create(requestedObject, container);

			// Verify outcome
			Assert.AreEqual(requestedObject, tracked, "Expected ingoing request to be tracked.");

			// Teardown
		}

		[TestMethod]
		public void CreateWillTrackRequestsInCompositeBuilderPipeline()
		{
			// Fixture setup
			object requestedObject = "The request";
			object subRequest = "Some sub request";
			var tracked = new List<object>();
			var sut = new DelegatingRequestTracker { OnTrackRequest = tracked.Add };
			var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
			var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
			var compBuilder = new CompositeSpecimenBuilder(sut, builder2, builder3);

			// Exercise system
			sut.Create(requestedObject, new DefaultSpecimenContainer(compBuilder));

			// Verify outcome
			Assert.AreEqual(2, tracked.Count, "Expected 2 requests tracked.");
			Assert.AreEqual(subRequest, tracked[1]);

			// Teardown
		}

		[TestMethod]
		public void CreateWillTrackCreatedSpecimen()
		{
			// Fixture setup
			object tracked = null;
			object createdSpecimen = Guid.NewGuid();
			var container = new DelegatingSpecimenContainer { OnCreate = r => createdSpecimen };
			var sut = new DelegatingRequestTracker { OnTrackCreatedSpecimen = r => tracked = r };

			// Exercise system
			object res = sut.Create(new object(), container);

			// Verify outcome
			Assert.AreEqual(res, tracked, "Expected created specimen to be tracked.");

			// Teardown
		}

		[TestMethod]
		public void CreateWillTrackCreatedSpecimensInCompositeBuilderPipeline()
		{
			// Fixture setup
			object requestedObject = "The request";
			object subRequest = "Some sub request";
			object createdSpecimen = Guid.NewGuid();
			var tracked = new List<object>();
			var sut = new DelegatingRequestTracker { OnTrackCreatedSpecimen = tracked.Add };
			var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Create(subRequest) : new NoSpecimen() };
			var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
			var compBuilder = new CompositeSpecimenBuilder(sut, builder2, builder3);

			// Exercise system
			sut.Create(requestedObject, new DefaultSpecimenContainer(compBuilder));

			// Verify outcome
			Assert.AreEqual(2, tracked.Count, "Expected created specimen tracked 2 times.");
			Assert.AreEqual(createdSpecimen, tracked[0]);
			Assert.AreEqual(createdSpecimen, tracked[1]);

			// Teardown
		}

		private class DelegatingRequestTracker : RequestTracker
		{
			public DelegatingRequestTracker()
			{
				this.OnTrackRequest = r => { };
				this.OnTrackCreatedSpecimen = r => { };
			}
			
			protected override void TrackRequest(object request)
			{
				OnTrackRequest(request);
			}

			protected override void TrackCreatedSpecimen(object specimen)
			{
				OnTrackCreatedSpecimen(specimen);
			}

			internal Action<object> OnTrackRequest { get; set; }
			internal Action<object> OnTrackCreatedSpecimen { get; set; }
		}
	}
}