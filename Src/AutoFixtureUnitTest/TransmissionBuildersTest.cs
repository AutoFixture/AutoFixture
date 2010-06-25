using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TransmissionBuildersTest
    {
        [Fact]
        public void SutIsSpecimenBuilders()
        {
            // Fixture setup
            // Exercise system
            var sut = new TransmissionBuilders();
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
                typeof(ModestConstructorInvoker),
                typeof(ParameterRequestTranslator),
                typeof(PropertyRequestTranslator),
                typeof(FieldRequestTranslator),
                typeof(ManyTranslator),
                typeof(FiniteSequenceUnwrapper),
                typeof(ValueIgnoringSeedUnwrapper)
            };
            // Exercise system
            var sut = new TransmissionBuilders();
            // Verify outcome
            Assert.True(expectedBuilderTypes.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }

        [Fact]
        public void SutIsMany()
        {
            // Fixture setup
            // Exercise system
            var sut = new TransmissionBuilders();
            // Verify outcome
            Assert.IsAssignableFrom<IMany>(sut);
            // Teardown
        }

        [Fact]
        public void CountIsProperWritableProperty()
        {
            // Fixture setup
            var sut = new TransmissionBuilders();
            var expectedCount = 912;
            // Exercise system
            sut.Count = expectedCount;
            var result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void SettingCountSetsCountOnManyTranslator()
        {
            // Fixture setup
            var sut = new TransmissionBuilders();
            var expectedCount = 76;
            // Exercise system
            sut.Count = expectedCount;
            // Verify outcome
            var manyTranslator = sut.OfType<ManyTranslator>().Single();
            Assert.Equal(expectedCount, manyTranslator.Count);
            // Teardown
        }

        [Fact]
        public void CountMatchesCountOnManyTranslator()
        {
            // Fixture setup
            var sut = new TransmissionBuilders();
            var expectedCount = 149;
            sut.OfType<ManyTranslator>().Single().Count = expectedCount;
            // Exercise system
            var result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }
    }
}
