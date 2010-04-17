namespace Ploeh.AutoFixtureUnitTest
{
	using System;
	using System.IO;
	using AutoFixture;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class BuildTraceWriterTest
	{
		[TestMethod]
		public void TrackRequestWritesToOutputStream()
		{
			// Fixture setup
			var traceStream = new StringWriter();
			object someRequestObject = Guid.NewGuid();
			var sut = new BuildTraceWriterInTest(traceStream);

			// Exercise system
			sut.InvokeTrackRequest(someRequestObject);
      traceStream.Close();

			// Verify outcome
			Assert.IsTrue(traceStream.ToString().Length > 0, "Expected something to be written to the output stream.");

			// Teardown
		}

		[TestMethod]
		public void TrackCreatedSpecimenWritesToOutputStream()
		{
			// Fixture setup
			var traceStream = new StringWriter();
			object someSpecimen = Guid.NewGuid();
			var sut = new BuildTraceWriterInTest(traceStream);

			// Exercise system
			sut.InvokeTrackCreatedSpecimen(someSpecimen);
			traceStream.Close();

			// Verify outcome
			Assert.IsTrue(traceStream.ToString().Length > 0, "Expected something to be written to the output stream.");

			// Teardown
		}

		[TestMethod]
		public void FormatterIsGivenRequestedObject()
		{
			// Fixture setup
			object someRequestObject = Guid.NewGuid();
			object objectToFormatter = null;
			var sut = new BuildTraceWriterInTest(new StringWriter());
			sut.TrackedRequestFormatter = (obj, i) =>
			                              	{
			                              		objectToFormatter = obj;
			                              		return null;
			                              	};

			// Exercise system
			sut.InvokeTrackRequest(someRequestObject);

			// Verify outcome
			Assert.AreEqual(someRequestObject, objectToFormatter, "Expected request object to go to formatter.");

			// Teardown
		}

		[TestMethod]
		public void FormatterDefaultsToToStringAndGetType()
		{
			// Fixture setup
			object someSpecimen = Guid.NewGuid();
			object objectToFormatter = null;
			var sut = new BuildTraceWriterInTest(new StringWriter());
			sut.TrackedCreatedSpecimenFormatter = (obj, i) =>
			{
				objectToFormatter = obj;
				return null;
			};

			// Exercise system
			sut.InvokeTrackCreatedSpecimen(someSpecimen);

			// Verify outcome
			Assert.AreEqual(someSpecimen, objectToFormatter, "Expected created specimen object to go to formatter.");

			// Teardown
		}

		[TestMethod]
		public void TrackRequestIncreasesIndentForNestedRequest()
		{
			// Fixture setup
			object someRequestObject = Guid.NewGuid();
			int lastRecordedIndent = 0;
			var sut = new BuildTraceWriterInTest(new StringWriter());
			sut.TrackedRequestFormatter = (obj, i) =>
			                              	{
			                              		lastRecordedIndent = i;
			                              		return null;
			                              	};
			int initialIndent = sut.IndentLevel;

			// Exercise system
			sut.InvokeTrackRequest(someRequestObject);
			sut.InvokeTrackRequest(someRequestObject);

			// Verify outcome

			Assert.AreEqual(initialIndent + 1, lastRecordedIndent);

			// Teardown
		}

		[TestMethod]
		public void TrackRequestDecreasesIndentForEachCreatedSpecimen()
		{
			// Fixture setup
			object someSpecimen = Guid.NewGuid();
			int lastRecordedIndent = 0;
			var sut = new BuildTraceWriterInTest(new StringWriter());
			sut.TrackedCreatedSpecimenFormatter = (obj, i) =>
			{
				lastRecordedIndent = i;
				return null;
			};
			int initialIndent = sut.IndentLevel;

			// Exercise system
			sut.InvokeTrackCreatedSpecimen(someSpecimen);
			sut.InvokeTrackCreatedSpecimen(someSpecimen);

			// Verify outcome

			Assert.AreEqual(initialIndent - 2, lastRecordedIndent);

			// Teardown
		}

		private class BuildTraceWriterInTest : BuildTraceWriter
		{
			public BuildTraceWriterInTest(TextWriter outStream) : base(outStream)
			{
			}

			public void InvokeTrackRequest(object request)
			{
				this.TrackRequest(request);
			}

			public void InvokeTrackCreatedSpecimen(object specimen)
			{
				this.TrackCreatedSpecimen(specimen);
			}
		}
	}
}