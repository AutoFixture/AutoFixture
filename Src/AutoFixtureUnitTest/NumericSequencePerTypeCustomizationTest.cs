using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class NumericSequencePerTypeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new NumericSequencePerTypeCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeWithNullThrowsArgumentNullException()
        {
            // Arrange
            var sut = new NumericSequencePerTypeCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsSpecializedNumericSpecimenBuildersToFixture()
        {
            // Arrange
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
            // Act
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<CompositeSpecimenBuilder>()
                .SelectMany(i => i)
                .Select(i => i.GetType());
            // Assert
            Assert.True(expectedBuilders.SequenceEqual(result));
        }

        [Fact]
        public void CreateAnonymousWithNumericSequencePerTypeCustomizationWillReturnCorrectValues()
        {
            // Arrange
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
            // Act
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
            // Assert
            Assert.True(expectedValues.SequenceEqual(results));
        }
    }
}