using System;
using System.IO;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TraceWriterTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyWriter = TextWriter.Null;
            var dummyBuilder = new DelegatingTracingBuilder();
            // Act
            var sut = new TraceWriter(dummyWriter, dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Arrange
            var dummyWriter = TextWriter.Null;
            var dummyBuilder = new DelegatingTracingBuilder();
            // Act
            var sut = new TraceWriter(dummyWriter, dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void InitializeWithNullWriterWillThrow()
        {
            // Arrange
            var dummyBuilder = new DelegatingTracingBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new TraceWriter(null, dummyBuilder));
        }

        [Fact]
        public void InitializeWithNullBuilderWillThrow()
        {
            // Arrange
            var dummyWriter = TextWriter.Null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new TraceWriter(dummyWriter, null));
        }

        [Fact]
        public void TracerIsCorrect()
        {
            // Arrange
            var dummyWriter = TextWriter.Null;
            var expectedTracer = new DelegatingTracingBuilder();
            var sut = new TraceWriter(dummyWriter, expectedTracer);
            // Act
            TracingBuilder result = sut.Tracer;
            // Assert
            Assert.Equal(expectedTracer, result);
        }

        [Fact]
        public void SutYieldsCorrectSequence()
        {
            // Arrange
            var dummyWriter = TextWriter.Null;
            var expected = new DelegatingSpecimenBuilder();
            var tracer = new DelegatingTracingBuilder(expected);
            var sut = new TraceWriter(dummyWriter, tracer);
            // Act
            // Assert
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var writer = TextWriter.Null;
            var tracer = new DelegatingTracingBuilder();
            var sut = new TraceWriter(writer, tracer);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var tw = Assert.IsAssignableFrom<TraceWriter>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(tw.Tracer.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var writer = TextWriter.Null;
            var tracer = new DelegatingTracingBuilder();
            var sut = new TraceWriter(writer, tracer);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var tw = Assert.IsAssignableFrom<TraceWriter>(actual);
            Assert.Equal(expected, tw.Tracer.Builder);
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Arrange
            var expectedSpecimen = new object();
            var stubBuilder = new TracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen });

            var dummyWriter = TextWriter.Null;
            var sut = new TraceWriter(dummyWriter, stubBuilder);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }

        [Fact]
        public void CreateWillInvokeDecoratedBuilderWithCorrectParameters()
        {
            // Arrange
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var mockBuilder = new TracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = expectedRequest == r && expectedContainer == c });

            var dummyWriter = TextWriter.Null;
            var sut = new TraceWriter(dummyWriter, mockBuilder);
            // Act
            sut.Create(expectedRequest, expectedContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void SpecimenRequestedWillWriteCorrectMessageToWriter()
        {
            // Arrange
            var writer = new StringWriter();
            var builder = new DelegatingTracingBuilder();

            var depth = new Random().Next(1, 10);
            var request = new object();

            var sut = new TraceWriter(writer, builder);
            // Act
            builder.RaiseSpecimenRequested(new RequestTraceEventArgs(request, depth));
            // Assert
            var expected = new string(' ', depth * 2) + "Requested: " + request + Environment.NewLine;
            Assert.Equal(expected, writer.ToString());
        }

        [Fact]
        public void SpecimenCreatedWillWriteCorrectMessageToWriter()
        {
            // Arrange
            var writer = new StringWriter();
            var builder = new DelegatingTracingBuilder();

            var depth = new Random().Next(1, 10);
            var specimen = new object();

            var sut = new TraceWriter(writer, builder);
            // Act
            var dummyRequest = new object();
            builder.RaiseSpecimenCreated(new SpecimenCreatedEventArgs(dummyRequest, specimen, depth));
            // Assert
            var expected = new string(' ', depth * 2) + "Created: " + specimen + Environment.NewLine;
            Assert.Equal(expected, writer.ToString());
        }

        [Fact]
        public void RequestFormatterIsGivenCorrectTextWriter()
        {
            // Arrange
            var expectedWriter = new StringWriter();
            var sut = new TraceWriter(expectedWriter, new DelegatingTracingBuilder());

            bool verified = false;
            sut.TraceRequestFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void SpecimenFormatterIsGivenCorrectTextWriter()
        {
            // Arrange
            var expectedWriter = new StringWriter();
            var sut = new TraceWriter(expectedWriter, new DelegatingTracingBuilder());

            bool verified = false;
            sut.TraceSpecimenFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void AssignNullRequestFormatterWillThrow()
        {
            // Arrange
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceRequestFormatter = null);
        }

        [Fact]
        public void AssignNullSpecimenFormatterWillThrow()
        {
            // Arrange
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceSpecimenFormatter = null);
        }

        [Fact]
        public void RequestFormatterIsProperWritableProperty()
        {
            // Arrange
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Act
            sut.TraceRequestFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceRequestFormatter;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SpecimenFormatterIsProperWritableProperty()
        {
            // Arrange
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new TraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Act
            sut.TraceSpecimenFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceSpecimenFormatter;
            // Assert
            Assert.Equal(expected, result);
        }
    }
}