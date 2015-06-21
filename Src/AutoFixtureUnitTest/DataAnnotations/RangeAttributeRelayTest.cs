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
        public void SutInheritsCommonLogicFromAttributeRelayWhichIsTestedSeparately()
        {
            // Fixture setup
            // Exercise system
            var sut = new RangeAttributeRelay();
            // Verify outcome
            Assert.IsAssignableFrom<AttributeRelay<RangeAttribute>>(sut);
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
        public void CreateResolvedRequestWithRangeAttributeReturnsCorrectResult(Type type, object minimum, object maximum)
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
            var sut = new TestableRangeAttributeRelay();
            // Exercise system
            var result = sut.CreateRelayedRequest(request, rangeAttribute);
            // Verify outcome
            Assert.Equal(expectedRequest, result);
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
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
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
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
            };
            var sut = new RangeAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private class TestableRangeAttributeRelay : RangeAttributeRelay
        {
            public new object CreateRelayedRequest(ICustomAttributeProvider request, RangeAttribute attribute)
            {
                return base.CreateRelayedRequest(request, attribute);
            }
        }
    }
}