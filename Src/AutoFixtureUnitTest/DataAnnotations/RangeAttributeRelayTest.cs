using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class RangeAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RangeAttributeRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RangeAttributeRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new RangeAttributeRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RangeAttributeRelay();
            var dummyRequest = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void CreateWithNonRangeAttributeRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new RangeAttributeRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int), 10, 20)]
        [InlineData(typeof(int), -2, -1)]
        [InlineData(typeof(decimal), 10, 20)]
        [InlineData(typeof(decimal), -2, -1)]
        [InlineData(typeof(double), 10, 20)]
        [InlineData(typeof(double), -2, -1)]
        [InlineData(typeof(long), 10, 20)]
        [InlineData(typeof(long), -2, -1)]
        public void CreateWithRangeAttributeRequestReturnsCorrectResult(Type type, object minimum, object maximum)
        {
            // Fixture setup
            var rangeAttribute = new RangeAttribute(type, minimum.ToString(), maximum.ToString());
            var providedAttribute = new ProvidedAttribute(rangeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            Type conversionType = rangeAttribute.OperandType;
            var expectedRequest = new RangedNumberRequest(
                conversionType,
                Convert.ChangeType(rangeAttribute.Minimum, conversionType, CultureInfo.CurrentCulture),
                Convert.ChangeType(rangeAttribute.Maximum, conversionType, CultureInfo.CurrentCulture)
                );
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(nameof(RangeValidatedType.Property))]
        [InlineData(nameof(RangeValidatedType.NullableTypeProperty))]
        public void CreateWithPropertyDecoratedWithRangeAttributeReturnsCorrectResult(string name)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetProperty(name);
            Type target = Nullable.GetUnderlyingType(request.PropertyType) 
                ?? request.PropertyType;

            var expectedRequest = new RangedNumberRequest(
                target,
                Convert.ChangeType(RangeValidatedType.Minimum, target, CultureInfo.CurrentCulture),
                Convert.ChangeType(RangeValidatedType.Maximum, target, CultureInfo.CurrentCulture)
                );
           
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(nameof(RangeValidatedType.Field))]
        [InlineData(nameof(RangeValidatedType.NullableTypeField))]
        public void CreateWithFieldDecoratedWithRangeAttributeReturnsCorrectResult(string name)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetField(name);
            Type target = Nullable.GetUnderlyingType(request.FieldType)
                ?? request.FieldType;

            var expectedRequest = new RangedNumberRequest(
                target,
                Convert.ChangeType(RangeValidatedType.Minimum, target, CultureInfo.CurrentCulture),
                Convert.ChangeType(RangeValidatedType.Maximum, target, CultureInfo.CurrentCulture)
                );

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Range(0, long.MaxValue)]
        public static long FieldWithOverflowedRange = 0;
        
        [Fact]
        public void FailsWithMeaningfulExceptionWhenBoundaryCannotBeConvertedWithoutOverflow()
        {
            // Fixture setup
            var request = typeof(RangeAttributeRelayTest).GetField(nameof(FieldWithOverflowedRange));
            
            var sut = new RangeAttributeRelay();
            var dummyContext = new DelegatingSpecimenContext();

            // Exercise system and verify outcome
            var actualEx = Assert.Throws<OverflowException>(() => sut.Create(request, dummyContext));
            Assert.Contains("To solve the issue", actualEx.Message);
            // Teardown
        }

        [Range(typeof(long), /* long.MinValue */ "-9223372036854775808", /* long.MaxValue */ "9223372036854775807")]
        public static long FieldWithStringValueRange = 0;

        [Fact]
        public void ShouldNotFailIfRangeIsSpecifiedAsString()
        {
            // Fixture setup
            var request = typeof(RangeAttributeRelayTest).GetField(nameof(FieldWithStringValueRange));

            var sut = new RangeAttributeRelay();
            var dummyContext = new DelegatingSpecimenContext();

            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Create(request, dummyContext)));
            
            // Teardown
        }

    }
}