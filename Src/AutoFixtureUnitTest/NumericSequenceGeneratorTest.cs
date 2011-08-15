using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class NumericSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new NumericSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(String))]
        [InlineData(typeof(Object))]
        [InlineData(typeof(Boolean))]
        public void CreateWithNonNumericTypeRequestReturnsNoSpecimen(Type request)
        {
            // Fixture setup
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Byte))]
        [InlineData(typeof(Decimal))]
        [InlineData(typeof(Double))]
        [InlineData(typeof(Int16))]
        [InlineData(typeof(Int32))]
        [InlineData(typeof(Int64))]
        [InlineData(typeof(SByte))]
        [InlineData(typeof(Single))]
        [InlineData(typeof(UInt16))]
        [InlineData(typeof(UInt32))]
        [InlineData(typeof(UInt64))]
        public void CreateWithNumericTypeRequestReturnsCorrectValue(Type request)
        {
            // Fixture setup
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            Assert.IsType(request, result);
            // Teardown
        }

        [Fact]
        public void CreateWith256ByteRequestsReturnsByteSpecimens()
        {
            // Fixture setup
            var sequence = Enumerable.Range(0, Byte.MaxValue + 1);
            var request = typeof(Byte);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sequence.Select(i => sut.Create(request, dummyContainer));
            // Verify outcome
            Assert.True(result.All(i => i.GetType() == request));
            // Teardown
        }

        [Fact]
        public void CreateWith128SByteRequestsReturnsSByteSpecimens()
        {
            // Fixture setup
            var sequence = Enumerable.Range(0, SByte.MaxValue + 1);
            var request = typeof(SByte);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sequence.Select(i => sut.Create(request, dummyContainer));
            // Verify outcome
            Assert.True(result.All(i => i.GetType() == request));
            // Teardown
        }
    }
}