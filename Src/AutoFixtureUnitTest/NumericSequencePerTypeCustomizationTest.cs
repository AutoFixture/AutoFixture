using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class NumericSequencePerTypeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new NumericSequencePerTypeCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullThrowsArgumentNullException()
        {
            // Fixture setup
            var sut = new NumericSequencePerTypeCustomization();
            // Exercise system and verify outcome
            Assert.Throws(typeof(ArgumentNullException), () => sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsSpecializedNumericSpecimenBuildersToFixture()
        {
            // Fixture setup
            var expectedBuilders = new[]
            {
                typeof(ByteSequenceGenerator),
                typeof(DecimalSequenceGenerator),
                typeof(DoubleSequenceGenerator),
                typeof(Int16SequenceGenerator),
                typeof(Int32SequenceGenerator),
                typeof(Int64SequenceGenerator),
                typeof(SByteSequenceGenerator),
                typeof(SingleSequenceGenerator),
                typeof(UInt16SequenceGenerator),
                typeof(UInt32SequenceGenerator),
                typeof(UInt64SequenceGenerator)
            };
            var fixture = new Fixture();
            var sut = new NumericSequencePerTypeCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<CompositeSpecimenBuilder>()
                .SelectMany(i => i.Builders)
                .Select(i => i.GetType());
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(result));
            // Teardown
        }
    }
}