using System;
using System.IO;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TraceWriterTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            var dummyBuilder = new DelegatingTracingBuilder();
            // Exercise system
            var sut = new TraceWriter(dummyWriter, dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            var dummyBuilder = new DelegatingTracingBuilder();
            // Exercise system
            var sut = new TraceWriter(dummyWriter, dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullWriterWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingTracingBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TraceWriter(null, dummyBuilder));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderWillThrow()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TraceWriter(dummyWriter, null));
            // Teardown
        }

        [Fact]
        public void TracerIsCorrect()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            var expectedTracer = new DelegatingTracingBuilder();
            var sut = new TraceWriter(dummyWriter, expectedTracer);
            // Exercise system
            TracingBuilder result = sut.Tracer;
            // Verify outcome
            Assert.Equal(expectedTracer, result);
            // Teardown
        }

        [Fact]
        public void SutYieldsCorrectSequence()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            var expected = new DelegatingSpecimenBuilder();
            var tracer = new DelegatingTracingBuilder(expected);
            var sut = new TraceWriter(dummyWriter, tracer);
            // Exercise system
            // Verify outcome
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var writer = TextWriter.Null;
            var tracer = new DelegatingTracingBuilder();
            var sut = new TraceWriter(writer, tracer);
            // Exercise system
            var expectedBuilders = new []
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var tw = Assert.IsAssignableFrom<TraceWriter>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(tw.Tracer.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var writer = TextWriter.Null;
            var tracer = new DelegatingTracingBuilder();
            var sut = new TraceWriter(writer, tracer);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var tw = Assert.IsAssignableFrom<TraceWriter>(actual);
            Assert.Equal(expected, tw.Tracer.Builder);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var stubBuilder = new TracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen });

            var dummyWriter = TextWriter.Null;
            var sut = new TraceWriter(dummyWriter, stubBuilder);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeDecoratedBuilderWithCorrectParameters()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var mockBuilder = new TracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = expectedRequest == r && expectedContainer == c });

            var dummyWriter = TextWriter.Null;
            var sut = new TraceWriter(dummyWriter, mockBuilder);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void SpecimenRequestedWillWriteCorrectMessageToWriter()
        {
            // Fixture setup
            var writer = new StringWriter();
            var builder = new DelegatingTracingBuilder();

            var depth = new Random().Next(1, 10);
            var request = new object();

            var sut = new TraceWriter(writer, builder);
            // Exercise system
            builder.RaiseSpecimenRequested(new RequestTraceEventArgs(request, depth));
            // Verify outcome
            var expected = new string(' ', depth * 2) + "Requested: " + request + Environment.NewLine;
            Assert.Equal(expected, writer.ToString());
            // Teardown
        }

        [Fact]
        public void SpecimenCreatedWillWriteCorrectMessageToWriter()
        {
            // Fixture setup
            var writer = new StringWriter();
            var builder = new DelegatingTracingBuilder();

            var depth = new Random().Next(1, 10);
            var specimen = new object();

            var sut = new TraceWriter(writer, builder);
            // Exercise system
            var dummyRequest = new object();
            builder.RaiseSpecimenCreated(new SpecimenCreatedEventArgs(dummyRequest, specimen, depth));
            // Verify outcome
            var expected = new string(' ', depth * 2) + "Created: " + specimen + Environment.NewLine;
            Assert.Equal(expected, writer.ToString());
            // Teardown
        }

        [Fact]
        public void RequestFormatterIsGivenCorrectTextWriter()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new TraceWriter(expectedWriter, new DelegatingTracingBuilder());

            bool verified = false;
            sut.TraceRequestFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void SpecimenFormatterIsGivenCorrectTextWriter()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new TraceWriter(expectedWriter, new DelegatingTracingBuilder());

            bool verified = false;
            sut.TraceSpecimenFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void AssignNullRequestFormatterWillThrow()
        {
            // Fixture setup
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceRequestFormatter = null);
            // Teardown
        }

        [Fact]
        public void AssignNullSpecimenFormatterWillThrow()
        {
            // Fixture setup
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceSpecimenFormatter = null);
            // Teardown
        }

        [Fact]
        public void RequestFormatterIsProperWritableProperty()
        {
            // Fixture setup
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
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
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system
            sut.TraceSpecimenFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceSpecimenFormatter;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}