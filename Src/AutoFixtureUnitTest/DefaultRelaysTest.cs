using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DefaultRelaysTest
    {
        [Fact]
        public void SutIsSpecimenBuilders()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultRelays();
            // Verify outcome
            Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
            // Teardown
        }

        [Fact]
        public void SutHasCorrectContents()
        {
            // Fixture setup
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
            // Exercise system
            var sut = new DefaultRelays();
            // Verify outcome
            Assert.True(expectedBuilderTypes.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }

        [Fact]
        public void NonGenericEnumeratorMatchesGenericEnumerator()
        {
            // Fixture setup
            var sut = new DefaultRelays();
            // Exercise system
            IEnumerable result = sut;
            // Verify outcome
            Assert.True(sut.Select(b => b.GetType()).SequenceEqual(result.Cast<object>().Select(o => o.GetType())));
            // Teardown
        }

        [Fact]
        public void SutIsMany()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultRelays();
            // Verify outcome
            Assert.IsAssignableFrom<IMultiple>(sut);
            // Teardown
        }

        [Fact]
        public void CountIsProperWritableProperty()
        {
            // Fixture setup
            var sut = new DefaultRelays();
            var expectedCount = 912;
            // Exercise system
            sut.Count = expectedCount;
            var result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }
    }
}
