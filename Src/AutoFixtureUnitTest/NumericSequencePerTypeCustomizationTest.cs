using System;
using System.Linq;
using Ploeh.AutoFixture;
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
        public void CreateAnonymousWithAllNumericTypesDoesNotReturnSequence()
        {
            // Fixture setup
            var sequence = new object[]
            {
                (byte)1,
                2M,
                3.0D,
                (short)4,
                5,
                (long)6,
                (sbyte)7,
                8.0F,
                (ushort)9,
                (uint)10,
                (ulong)11
            };
            var fixture = new Fixture();
            var sut = new NumericSequencePerTypeCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = new object[]
            {
                fixture.CreateAnonymous<byte>(),
                fixture.CreateAnonymous<decimal>(),
                fixture.CreateAnonymous<double>(),
                fixture.CreateAnonymous<short>(),
                fixture.CreateAnonymous<int>(),
                fixture.CreateAnonymous<long>(),
                fixture.CreateAnonymous<sbyte>(),
                fixture.CreateAnonymous<float>(),
                fixture.CreateAnonymous<ushort>(),
                fixture.CreateAnonymous<uint>(),
                fixture.CreateAnonymous<ulong>()
            };
            // Verify outcome
            Assert.False(sequence.SequenceEqual(result));
            // Teardown
        }
    }
}