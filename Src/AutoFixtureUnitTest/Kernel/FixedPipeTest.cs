using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FixedPipeTest
    {
        [Fact]
        public void SutIsPipe()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new FixedPipe(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderPipe>(sut);
            // Teardown
        }

        [Fact]
        public void PipeReturnsCorrectResult()
        {
            // Fixture setup
            var encapsulatedBuilder = new DelegatingSpecimenBuilder();
            var sut = new FixedPipe(encapsulatedBuilder);
            var builders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            // Exercise system
            var result = sut.Pipe(builders);
            // Verify outcome
            Assert.True(new ISpecimenBuilder[] { encapsulatedBuilder }
                .Concat(builders)
                .SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void PipeNullThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FixedPipe(dummyBuilder);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Pipe(null));
            // Teardown
        }
    }
}
