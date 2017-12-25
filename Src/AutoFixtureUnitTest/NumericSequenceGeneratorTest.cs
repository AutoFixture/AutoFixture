using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class NumericSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new NumericSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new NumericSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var sut = new NumericSequenceGenerator();
            // Act & assert
            var dummyRequest = new object();
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var sut = new NumericSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(bool))]
        public void CreateWithNonNumericTypeRequestReturnsNoSpecimen(Type request)
        {
            // Arrange
            var sut = new NumericSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(byte))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(double))]
        [InlineData(typeof(short))]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(float))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(ulong))]
        public void CreateWithNumericTypeRequestReturnsCorrectValue(Type request)
        {
            // Arrange
            var sut = new NumericSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Assert
            Assert.IsType(request, result);
        }

        [Fact]
        public void CreateWith256ByteRequestsReturnsByteSpecimens()
        {
            // Arrange
            var sequence = Enumerable.Range(0, Byte.MaxValue + 1);
            var request = typeof(Byte);
            var sut = new NumericSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sequence.Select(i => sut.Create(request, dummyContainer));
            // Assert
            Assert.True(result.All(i => i.GetType() == request));
        }

        [Fact]
        public void CreateWith128SByteRequestsReturnsSByteSpecimens()
        {
            // Arrange
            var sequence = Enumerable.Range(0, SByte.MaxValue + 1);
            var request = typeof(SByte);
            var sut = new NumericSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sequence.Select(i => sut.Create(request, dummyContainer));
            // Assert
            Assert.True(result.All(i => i.GetType() == request));
        }
    }
}
