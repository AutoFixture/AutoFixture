namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.IO;
    using AutoFixture;
    using Xunit;
    using Ploeh.AutoFixtureUnitTest.Kernel;
    using Ploeh.AutoFixture.Kernel;

    public class BuildTraceWriterTest
    {
        [Fact]
        public void DefaultIndentIsCorrect()
        {
            // Fixture setup
            var sut = new BuildTraceWriter(TextWriter.Null, new DelegatingSpecimenBuilder());
            // Exercise system
            var result = sut.Depth;
            // Verify outcome
            Assert.Equal(0, result);
            // Teardown
        }

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
        public void RequestFormatterIsGivenRequestedObject()
        {
            // Fixture setup
            object someRequestObject = Guid.NewGuid();
            object objectToFormatter = null;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceRequestFormatter = (tw, obj, i) => objectToFormatter = obj;

            // Exercise system
            sut.InvokeTrackRequest(someRequestObject);

            // Verify outcome
            Assert.Equal(someRequestObject, objectToFormatter);

            // Teardown
        }

        [Fact]
        public void RequestFormatterIsGivenCorrectTextWriter()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new BuildTraceWriterInTest(expectedWriter);

            bool verified = false;
            sut.TraceRequestFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Exercise system
            var dummyRequest = new object();
            sut.InvokeTrackRequest(dummyRequest);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void SpecimenFormatterIsGivenCorrectTextWriter()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new BuildTraceWriterInTest(expectedWriter);

            bool verified = false;
            sut.TraceCreatedSpecimenFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Exercise system
            var dummyRequest = new object();
            sut.InvokeTrackCreatedSpecimen(dummyRequest);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void FormatterDefaultsToToStringAndGetType()
        {
            // Fixture setup
            object someSpecimen = Guid.NewGuid();
            object objectToFormatter = null;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceCreatedSpecimenFormatter = (tw, obj, i) => objectToFormatter = obj;

            // Exercise system
            sut.InvokeTrackCreatedSpecimen(someSpecimen);

            // Verify outcome
            Assert.Equal(someSpecimen, objectToFormatter);

            // Teardown
        }

        [Fact]
        public void IndentLevelIsCorrectlyModifiedByTrackRequest()
        {
            // Fixture setup
            var sut = new BuildTraceWriterInTest(TextWriter.Null);
            // Exercise system
            var dummyRequest = new object();
            sut.InvokeTrackRequest(dummyRequest);
            // Verify outcome
            Assert.Equal(1, sut.Depth);
            // Teardown
        }

        [Fact]
        public void TrackRequestIncreasesIndentForNestedRequest()
        {
            // Fixture setup
            object someRequestObject = Guid.NewGuid();
            int lastRecordedIndent = 0;
            var sut = new BuildTraceWriterInTest(new StringWriter());
            sut.TraceRequestFormatter = (tw, obj, i) => lastRecordedIndent = i;
            int initialIndent = sut.Depth;

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
            sut.TraceCreatedSpecimenFormatter = (tw, obj, i) => lastRecordedIndent = i;
            int initialIndent = sut.Depth;

            // Exercise system
            sut.InvokeTrackCreatedSpecimen(someSpecimen);
            sut.InvokeTrackCreatedSpecimen(someSpecimen);

            // Verify outcome

            Assert.Equal(initialIndent - 2, lastRecordedIndent);

            // Teardown
        }

        [Fact]
        public void AssignNullRequestFormatterWillThrow()
        {
            // Fixture setup
            var sut = new BuildTraceWriterInTest(TextWriter.Null);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceRequestFormatter = null);
            // Teardown
        }

        [Fact]
        public void AssignNullSpecimenFormatterWillThrow()
        {
            // Fixture setup
            var sut = new BuildTraceWriterInTest(TextWriter.Null);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceCreatedSpecimenFormatter = null);
            // Teardown
        }

        [Fact]
        public void RequestFormatterIsProperWritableProperty()
        {
            // Fixture setup
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new BuildTraceWriterInTest(TextWriter.Null);
            // Exercise system
            sut.TraceRequestFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceRequestFormatter;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SpecimenFormatterIsProperWritableProperty()
        {
            // Fixture setup
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new BuildTraceWriterInTest(TextWriter.Null);
            // Exercise system
            sut.TraceCreatedSpecimenFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceCreatedSpecimenFormatter;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        private class BuildTraceWriterInTest : BuildTraceWriter
        {
            public BuildTraceWriterInTest(TextWriter writer)
                : this(writer, new DelegatingSpecimenBuilder())
            {
            }

            public BuildTraceWriterInTest(TextWriter writer, ISpecimenBuilder builder)
                : base(writer, builder)
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