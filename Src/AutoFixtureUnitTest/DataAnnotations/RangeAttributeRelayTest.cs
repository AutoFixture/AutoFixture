using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            // Arrange
            // Act
            var sut = new RangeAttributeRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RangeAttributeRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new RangeAttributeRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RangeAttributeRelay();
            var dummyRequest = new object();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
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
            // Arrange
            var sut = new RangeAttributeRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
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
            // Arrange
            var rangeAttribute = new RangeAttribute(type, minimum.ToString(), maximum.ToString());
            var providedAttribute = new ProvidedAttribute(rangeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            Type conversionType = rangeAttribute.OperandType;
            var expectedRequest = new RangedRequest(conversionType, conversionType, rangeAttribute.Minimum,
                rangeAttribute.Maximum);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(nameof(RangeValidatedType.Property), typeof(decimal), typeof(int))]
        [InlineData(nameof(RangeValidatedType.NullableTypeProperty), typeof(decimal), typeof(int))]
        public void CreateWithPropertyDecoratedWithRangeAttributeReturnsCorrectResult(
            string name, Type expectedMemberType, Type expectedOperandType)
        {
            // Arrange
            var request = typeof(RangeValidatedType).GetProperty(name);

            var expectedRequest = new RangedRequest(
                expectedMemberType, expectedOperandType, RangeValidatedType.Minimum, RangeValidatedType.Maximum);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(nameof(RangeValidatedType.Field), typeof(decimal), typeof(int))]
        [InlineData(nameof(RangeValidatedType.NullableTypeField), typeof(decimal), typeof(int))]
        public void CreateWithFieldDecoratedWithRangeAttributeReturnsCorrectResult(
            string name, Type expectedMemberType, Type expectedOperandType)
        {
            // Arrange
            var request = typeof(RangeValidatedType).GetField(name);

            var expectedRequest = new RangedRequest(
                expectedMemberType, expectedOperandType, RangeValidatedType.Minimum, RangeValidatedType.Maximum);

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(nameof(RangeValidatedType.MethodWithRangedParameter), typeof(decimal), typeof(int))]
        [InlineData(nameof(RangeValidatedType.MethodWithRangedNullableParameter), typeof(decimal), typeof(int))]
        public void CreateWithParameterDecoratedWithRangeAttributeReturnsCorrectResult(
            string methodName, Type expectedMemberType, Type expectedOperandType)
        {
            // Arrange
            var request = typeof(RangeValidatedType).GetMethod(methodName).GetParameters().Single();

            var expectedRequest = new RangedRequest(
                expectedMemberType, expectedOperandType, RangeValidatedType.Minimum, RangeValidatedType.Maximum);

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new RangeAttributeRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}