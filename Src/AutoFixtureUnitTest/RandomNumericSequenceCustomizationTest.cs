using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomNumericSequenceCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomNumericSequenceCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullFixtureThrows()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCorrectBuildersToFixture()
        {
            // Fixture setup
            var expectedResult = new[]
            {
                typeof(RandomNumericSequenceLimitGenerator),
                typeof(RandomNumericSequenceGenerator)
            };
            var fixture = new Fixture();
            var sut = new RandomNumericSequenceCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<CompositeSpecimenBuilder>()
                .SelectMany(i => i.Builders)
                .Select(i => i.GetType());
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithRandomNumericSequenceCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture()
                .Customize(new RandomNumericSequenceCustomization());
            var numbers = new object[]
            {
                1,
                (uint)2,
                (byte)3,
                (sbyte)4,
                (long)5,
                (ulong)6,
                (short)7,
                (ushort)8,
                9.0F,
                10.0D,
                11M
            };
            // Exercise system
            var result = new object[]
            {
                sut.CreateAnonymous<int>(),
                sut.CreateAnonymous<uint>(),
                sut.CreateAnonymous<byte>(),
                sut.CreateAnonymous<sbyte>(),
                sut.CreateAnonymous<long>(),
                sut.CreateAnonymous<ulong>(),
                sut.CreateAnonymous<short>(),
                sut.CreateAnonymous<ushort>(),
                sut.CreateAnonymous<float>(),
                sut.CreateAnonymous<double>(),
                sut.CreateAnonymous<decimal>()
            };
            // Verify outcome
            Assert.False(numbers.SequenceEqual(result));
            // Teardown
        }
    }
}