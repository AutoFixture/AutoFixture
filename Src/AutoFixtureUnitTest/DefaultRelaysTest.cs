using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using System.Collections;

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
                typeof(ConstructorInvoker),
                typeof(ParameterRequestRelay),
                typeof(PropertyRequestRelay),
                typeof(FieldRequestRelay),
                typeof(ManyRelay),
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
            Assert.IsAssignableFrom<IMany>(sut);
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

        [Fact]
        public void SettingCountSetsCountOnManyRelay()
        {
            // Fixture setup
            var sut = new DefaultRelays();
            var expectedCount = 76;
            // Exercise system
            sut.Count = expectedCount;
            // Verify outcome
            var manyRelay = sut.OfType<ManyRelay>().Single();
            Assert.Equal(expectedCount, manyRelay.Count);
            // Teardown
        }

        [Fact]
        public void CountMatchesCountOnManyRelay()
        {
            // Fixture setup
            var sut = new DefaultRelays();
            var expectedCount = 149;
            sut.OfType<ManyRelay>().Single().Count = expectedCount;
            // Exercise system
            var result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }
    }
}
