using System;
using System.IO;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TracingBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new TracingBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullWriterThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TracingBehavior(null));
            // Teardown
        }

        [Fact]
        public void WriterIsCorrectWhenExplicitlyProvided()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new TracingBehavior(expectedWriter);
            // Exercise system
            TextWriter result = sut.Writer;
            // Verify outcome
            Assert.Equal(expectedWriter, result);
            // Teardown
        }

        [Fact]
        public void WriterIsCorrectWhenDefaultConstructorIsUsed()
        {
            // Fixture setup
            var sut = new TracingBehavior();
            // Exercise system
            var result = sut.Writer;
            // Verify outcome
            Assert.Equal(Console.Out, result);
            // Teardown
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Fixture setup
            var sut = new TracingBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new TracingBehavior();
            var builder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(builder);
            // Verify outcome
            var tw = Assert.IsAssignableFrom<TraceWriter>(result);
            Assert.Equal(builder, tw.Tracer.Builder);
            // Teardown
        }
    }
}
