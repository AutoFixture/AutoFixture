using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

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

        [Fact]
        public void CreateWithNullContainerThrowsArgumentNullException()
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new NumericSequenceGenerator();
            // Exercise system and verify outcome
            Assert.Throws(typeof(ArgumentNullException), () => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonNumericRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var nonNumericRequest = new object();
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonNumericRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonNumericRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithByteRequestReturnsByteValue()
        {
            // Fixture setup
            var byteRequest = typeof(Byte);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(byteRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Byte>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDecimalRequestReturnsDecimalValue()
        {
            // Fixture setup
            var decimalRequest = typeof(Decimal);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(decimalRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Decimal>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDoubleRequestReturnsDoubleValue()
        {
            // Fixture setup
            var doubleRequest = typeof(Double);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(doubleRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Double>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt16RequestReturnsInt16Value()
        {
            // Fixture setup
            var shortRequest = typeof(Int16);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(shortRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Int16>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt32RequestReturnsInt32Value()
        {
            // Fixture setup
            var intRequest = typeof(Int32);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(intRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Int32>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt64RequestReturnsInt64Value()
        {
            // Fixture setup
            var longRequest = typeof(Int64);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(longRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Int64>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithSByteRequestReturnsSByteValue()
        {
            // Fixture setup
            var sbyteRequest = typeof(SByte);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(sbyteRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<SByte>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithSingleRequestReturnsSingleValue()
        {
            // Fixture setup
            var singleRequest = typeof(Single);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(singleRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Single>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt16RequestReturnsUInt16Value()
        {
            // Fixture setup
            var ushortRequest = typeof(UInt16);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(ushortRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<UInt16>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt32RequestReturnsUInt32Value()
        {
            // Fixture setup
            var uintRequest = typeof(UInt32);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uintRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<UInt32>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt64RequestReturnsUInt64Value()
        {
            // Fixture setup
            var ulongRequest = typeof(UInt64);
            var sut = new NumericSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(ulongRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<UInt64>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithByteRequest256TimesDoesNotThrowException()
        {
            // Fixture setup
            var byteRequest = typeof(Byte);
            var sut = new NumericSequenceGenerator();
            // Exercise system and verify outcome
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < Byte.MaxValue + 1; i++)
                {
                    sut.Create(byteRequest, dummyContainer);
                }
            });
            // Teardown
        }

        [Fact]
        public void CreateWithSByteRequest128TimesDoesNotThrowException()
        {
            // Fixture setup
            var sbyteRequest = typeof(SByte);
            var sut = new NumericSequenceGenerator();
            // Exercise system and verify outcome
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < SByte.MaxValue + 1; i++)
                {
                    sut.Create(sbyteRequest, dummyContainer);
                }
            });
            // Teardown
        }
    }
}