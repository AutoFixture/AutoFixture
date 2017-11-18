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
            var expectedRequest = new RangedRequest(conversionType, conversionType, rangeAttribute.Minimum,
                rangeAttribute.Maximum);
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
        [InlineData(nameof(RangeValidatedType.Property), typeof(decimal), typeof(int))]
        [InlineData(nameof(RangeValidatedType.NullableTypeProperty), typeof(decimal), typeof(int))]
        public void CreateWithPropertyDecoratedWithRangeAttributeReturnsCorrectResult(
            string name, Type expectedMemberType, Type expectedOperandType)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetProperty(name);

            var expectedRequest = new RangedRequest(
                expectedMemberType, expectedOperandType, RangeValidatedType.Minimum, RangeValidatedType.Maximum);
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
        [InlineData(nameof(RangeValidatedType.Field), typeof(decimal), typeof(int))]
        [InlineData(nameof(RangeValidatedType.NullableTypeField), typeof(decimal), typeof(int))]
        public void CreateWithFieldDecoratedWithRangeAttributeReturnsCorrectResult(
            string name, Type expectedMemberType, Type expectedOperandType)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetField(name);

            var expectedRequest = new RangedRequest(
                expectedMemberType, expectedOperandType, RangeValidatedType.Minimum, RangeValidatedType.Maximum);

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
        [InlineData(nameof(RangeValidatedType.MethodWithRangedParameter), typeof(decimal), typeof(int))]
        [InlineData(nameof(RangeValidatedType.MethodWithRangedNullableParameter), typeof(decimal), typeof(int))]
        public void CreateWithParameterDecoratedWithRangeAttributeReturnsCorrectResult(
            string methodName, Type expectedMemberType, Type expectedOperandType)
        {
            // Fixture setup
            var request = typeof(RangeValidatedType).GetMethod(methodName).GetParameters().Single();

            var expectedRequest = new RangedRequest(
                expectedMemberType, expectedOperandType, RangeValidatedType.Minimum, RangeValidatedType.Maximum);

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
    }
}