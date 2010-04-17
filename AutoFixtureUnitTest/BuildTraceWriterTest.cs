namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.IO;
    using AutoFixture;
    using Xunit;

    public class BuildTraceWriterTest
    {
        [Fact]
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
            Assert.True(traceStream.ToString().Length > 0, "Expected something to be written to the output stream.");

            // Teardown
        }

        [Fact]
        public void TrackCreatedSpecimenWritesToOutputStream()
        {
            // Fixture setup
            var traceStream = new StringWriter();
            object someSpecimen = Guid.NewGuid();
            var sut = new BuildTraceWriterInTest(traceStream);

            // Exercise system
            sut.InvokeTrackRequest(new object());
            sut.InvokeTrackCreatedSpecimen(someSpecimen);
            traceStream.Close();

            // Verify outcome
            Assert.True(traceStream.ToString().Length > 0, "Expected something to be written to the output stream.");

            // Teardown
        }

        [Fact]
        public void FormatterIsGivenRequestedObject()
        {
            // Fixture setup
            object someRequestObject = Guid.NewGuid();
            object objectToFormatter = null;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceRequestFormatter = (obj, i) =>
                                            {
                                                objectToFormatter = obj;
                                                return null;
                                            };

            // Exercise system
            sut.InvokeTrackRequest(someRequestObject);

            // Verify outcome
            Assert.Equal(someRequestObject, objectToFormatter);

            // Teardown
        }

        [Fact]
        public void FormatterDefaultsToToStringAndGetType()
        {
            // Fixture setup
            object someSpecimen = Guid.NewGuid();
            object objectToFormatter = null;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceCreatedSpecimenFormatter = (obj, i) =>
            {
                objectToFormatter = obj;
                return null;
            };

            // Exercise system
            sut.InvokeTrackCreatedSpecimen(someSpecimen);

            // Verify outcome
            Assert.Equal(someSpecimen, objectToFormatter);

            // Teardown
        }

        [Fact]
        public void TrackRequestIncreasesIndentForNestedRequest()
        {
            // Fixture setup
            object someRequestObject = Guid.NewGuid();
            int lastRecordedIndent = 0;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceRequestFormatter = (obj, i) =>
                                            {
                                                lastRecordedIndent = i;
                                                return null;
                                            };
            int initialIndent = sut.IndentLevel;

            // Exercise system
            sut.InvokeTrackRequest(someRequestObject);
            sut.InvokeTrackRequest(someRequestObject);

            // Verify outcome

            Assert.Equal(initialIndent + 1, lastRecordedIndent);

            // Teardown
        }

        [Fact]
        public void TrackRequestDecreasesIndentForEachCreatedSpecimen()
        {
            // Fixture setup
            object someSpecimen = Guid.NewGuid();
            int lastRecordedIndent = 0;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceCreatedSpecimenFormatter = (obj, i) =>
            {
                lastRecordedIndent = i;
                return null;
            };
            int initialIndent = sut.IndentLevel;

            // Exercise system
            sut.InvokeTrackCreatedSpecimen(someSpecimen);
            sut.InvokeTrackCreatedSpecimen(someSpecimen);

            // Verify outcome

            Assert.Equal(initialIndent - 2, lastRecordedIndent);

            // Teardown
        }

        private class BuildTraceWriterInTest : BuildTraceWriter
        {
            public BuildTraceWriterInTest(TextWriter outStream)
                : base(outStream)
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