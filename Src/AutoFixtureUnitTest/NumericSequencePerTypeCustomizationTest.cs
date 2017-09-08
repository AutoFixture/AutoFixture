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
                .SelectMany(i => i)
                .Select(i => i.GetType());
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithNumericSequencePerTypeCustomizationWillReturnCorrectValues()
        {
            // Fixture setup
            var expectedValues = new object[]
            {
                (byte)1,
                1M,
                1.0D,
                (short)1,
                1,
                (long)1,
                (sbyte)1,
                1.0F,
                (ushort)1,
                (uint)1,
                (ulong)1
            };
            var sut = new Fixture();
            var customization = new NumericSequencePerTypeCustomization();
            // Exercise system
            sut.Customize(customization);
            var results = new object[]
            {
                sut.Create<byte>(),
                sut.Create<decimal>(),
                sut.Create<double>(),
                sut.Create<short>(),
                sut.Create<int>(),
                sut.Create<long>(),
                sut.Create<sbyte>(),
                sut.Create<float>(),
                sut.Create<ushort>(),
                sut.Create<uint>(),
                sut.Create<ulong>()
            };
            // Verify outcome
            Assert.True(expectedValues.SequenceEqual(results));
            // Teardown
        }
    }
}