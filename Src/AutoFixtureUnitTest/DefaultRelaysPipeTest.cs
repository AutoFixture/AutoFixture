using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DefaultRelaysPipeTest
    {
        [Fact]
        public void SutIsSpecimenBuilderPipe()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultRelaysPipe();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderPipe>(sut);
            // Teardown
        }

        [Fact]
        public void PipeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new DefaultRelaysPipe();
            // Exercise system
            var dummyBuilders = Enumerable.Empty<ISpecimenBuilder>();
            var result = sut.Pipe(dummyBuilders);
            // Verify outcome
            var expectedBuilderTypes = new[]
            {                
                typeof(ArrayRelay),
                typeof(MethodInvoker),
                typeof(ParameterRequestRelay),
                typeof(RangeAttributeRelay),
                typeof(StringLengthAttributeRelay),
                typeof(PropertyRequestRelay),
                typeof(FieldRequestRelay),
                typeof(FiniteSequenceRelay),
                typeof(SeedIgnoringRelay)
            };
            Assert.True(expectedBuilderTypes.SequenceEqual(result.Select(b => b.GetType())));
            // Teardown
        }
    }
}
