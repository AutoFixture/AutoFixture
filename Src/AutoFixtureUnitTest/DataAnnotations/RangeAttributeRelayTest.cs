using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
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
#pragma warning disable 618
            var expectedResult = new NoSpecimen(dummyRequest);
#pragma warning restore 618
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
#pragma warning disable 618
            var expectedResult = new NoSpecimen(request);
#pragma warning restore 618
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
            ICustomAttributeProvider request = new FakeCustomAttributeProvider(providedAttribute);
            Type conversionType = rangeAttribute.OperandType;
            var expectedRequest = new RangedNumberRequest(
                conversionType,
                Convert.ChangeType(rangeAttribute.Minimum, conversionType, CultureInfo.CurrentCulture),
                Convert.ChangeType(rangeAttribute.Maximum, conversionType, CultureInfo.CurrentCulture)
                );
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
#pragma warning disable 618
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void CreateWithEnumRangeAttributeRequestReturnsCorrectResult(object minimum, object maximum)
        {
            // Fixture setup
            var rangeAttribute = new RangeAttribute(typeof(RangeValidatedEnum), minimum.ToString(), maximum.ToString());
            var providedAttribute = new ProvidedAttribute(rangeAttribute, true);
            ICustomAttributeProvider request = new FakeCustomAttributeProvider(providedAttribute);
            Type conversionType = rangeAttribute.OperandType;
            var expectedRequest = new RangedNumberRequest(
                conversionType,
                Enum.Parse(conversionType, rangeAttribute.Minimum.ToString()),
                Enum.Parse(conversionType, rangeAttribute.Maximum.ToString())
                );
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
#pragma warning disable 618
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }        

        [Theory]
        [InlineData("Property", 10, 20)]
        [InlineData("Property", -2, -1)]
        [InlineData("Property", "10.1", "20.2")]
        [InlineData("Property", "-2.2", "-1.1")]
        [InlineData("Property", 10.0, 20.0)]
        [InlineData("Property", -2.0, -1.0)]
        [InlineData("Property", 10, 20)]
        [InlineData("Property", -2, -1)]
        [InlineData("NullableTypeProperty", 10, 20)]
        [InlineData("NullableTypeProperty", -2, -1)]
        [InlineData("NullableTypeProperty", "10.1", "20.2")]
        [InlineData("NullableTypeProperty", "-2.2", "-1.1")]
        [InlineData("NullableTypeProperty", 10.0, 20.0)]
        [InlineData("NullableTypeProperty", -2.0, -1.0)]
        [InlineData("NullableTypeProperty", 10, 20)]
        [InlineData("NullableTypeProperty", -2, -1)]
        public void CreateWithPropertyDecoratedWithRangeAttributeReturnsCorrectResult(
            string name,
            object attributeMinimum,
            object attributeMaximum)
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
#pragma warning disable 618
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData("EnumProperty", 1, 2)]
        [InlineData("EnumProperty", 1, 3)]
        [InlineData("EnumProperty", 2, 3)]
        [InlineData("EnumProperty", "1", "2")]
        [InlineData("EnumProperty", "1", "3")]
        [InlineData("EnumProperty", "2", "3")]
        [InlineData("NullableTypeEnumProperty", 1, 2)]
        [InlineData("NullableTypeEnumProperty", 1, 3)]
        [InlineData("NullableTypeEnumProperty", 2, 3)]
        [InlineData("NullableTypeEnumProperty", "1", "2")]
        [InlineData("NullableTypeEnumProperty", "1", "3")]
        [InlineData("NullableTypeEnumProperty", "2", "3")]        
        public void CreateWithEnumPropertyDecoratedWithRangeAttributeReturnsCorrectResult(
            string name,
            object attributeMinimum,
            object attributeMaximum)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetProperty(name);
            Type target = Nullable.GetUnderlyingType(request.PropertyType)
                ?? request.PropertyType;

            var expectedRequest = new RangedNumberRequest(
                target,                
                Enum.Parse(target, RangeValidatedType.EnumMinimum.ToString()),
                Enum.Parse(target, RangeValidatedType.EnumMaximum.ToString())
                );

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
#pragma warning disable 618
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData("Field", 10, 20)]
        [InlineData("Field", -2, -1)]
        [InlineData("Field", "10.1", "20.2")]
        [InlineData("Field", "-2.2", "-1.1")]
        [InlineData("Field", 10.0, 20.0)]
        [InlineData("Field", -2.0, -1.0)]
        [InlineData("Field", 10, 20)]
        [InlineData("Field", -2, -1)]
        [InlineData("NullableTypeField", 10, 20)]
        [InlineData("NullableTypeField", -2, -1)]
        [InlineData("NullableTypeField", "10.1", "20.2")]
        [InlineData("NullableTypeField", "-2.2", "-1.1")]
        [InlineData("NullableTypeField", 10.0, 20.0)]
        [InlineData("NullableTypeField", -2.0, -1.0)]
        [InlineData("NullableTypeField", 10, 20)]
        [InlineData("NullableTypeField", -2, -1)]
        public void CreateWithFieldDecoratedWithRangeAttributeReturnsCorrectResult(
            string name,
            object attributeMinimum,
            object attributeMaximum)
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
#pragma warning disable 618
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData("EnumField", 1, 2)]
        [InlineData("EnumField", 1, 3)]
        [InlineData("EnumField", 2, 3)]
        [InlineData("EnumField", "1", "2")]
        [InlineData("EnumField", "1", "3")]
        [InlineData("EnumField", "2", "3")]
        [InlineData("NullableTypeEnumField", 1, 2)]
        [InlineData("NullableTypeEnumField", 1, 3)]
        [InlineData("NullableTypeEnumField", 2, 3)]
        [InlineData("NullableTypeEnumField", "1", "2")]
        [InlineData("NullableTypeEnumField", "1", "3")]
        [InlineData("NullableTypeEnumField", "2", "3")]
        public void CreateWithEnumFieldDecoratedWithRangeAttributeReturnsCorrectResult(
            string name,
            object attributeMinimum,
            object attributeMaximum)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetField(name);
            Type target = Nullable.GetUnderlyingType(request.FieldType)
                ?? request.FieldType;

            var expectedRequest = new RangedNumberRequest(
                target,
                Enum.Parse(target, RangeValidatedType.EnumMinimum.ToString()),
                Enum.Parse(target, RangeValidatedType.EnumMaximum.ToString())
                );

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
#pragma warning disable 618
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}