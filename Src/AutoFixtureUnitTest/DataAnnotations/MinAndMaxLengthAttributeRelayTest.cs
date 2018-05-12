using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class MinAndMaxLengthAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Act
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

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
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());
            var dummyRequest = new object();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());
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
        public void CreateWithNonConstrainedStringRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);

            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedbyMinLengthReturnsCorrectResult(int min)
        {
            // Arrange
            var minLengthAttribute = new MinLengthAttribute(min);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(min, Int32.MaxValue);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedbyMaxLengthReturnsCorrectResult(int max)
        {
            // Arrange
            var maxLengthAttribute = new MaxLengthAttribute(max);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(1, max);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        public void CreateWithStringRequestConstrainedbyMinAndMaxLengthReturnsCorrectResult(int min, int max)
        {
            // Arrange
            var minLengthAttribute = new MinLengthAttribute(min);
            var maxLengthAttribute = new MaxLengthAttribute(max);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true),
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(min, max);
            var expectedResult = new object();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        public void CreateWithFiniteSequenceRequestConstrainedbyMinAndMaxLengthReturnsCorrectResult(int min, int max)
        {
            // Arrange
            var memberType = typeof(string[]);
            var randomNumberWithinRange = (min + max) / 2;

            var minLengthAttribute = new MinLengthAttribute(min);
            var maxLengthAttribute = new MaxLengthAttribute(max);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true),
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRangedNumberRequest = new RangedNumberRequest(typeof(int), min, max);
            var expectedFiniteSequenceRequest = new FiniteSequenceRequest(
                memberType.GetElementType(),
                randomNumberWithinRange);

            var expectedResult = new List<string>();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (r.Equals(expectedRangedNumberRequest)) return randomNumberWithinRange;
                    if (r.Equals(expectedFiniteSequenceRequest)) return expectedResult;
                    return new NoSpecimen();
                }
            };

            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver { MemberType = memberType });

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 5, 1)]
        [InlineData(1, 5, 1)]
        [InlineData(2, 5, 2)]
        public void WhenMaxValueIsGreaterThen0MinShouldBeEqualToAtLeast1(int min, int max, int expectedMin)
        {
            // Arrange
            var minLengthAttribute = new MinLengthAttribute(min);
            var maxLengthAttribute = new MaxLengthAttribute(max);

            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true),
                new ProvidedAttribute(maxLengthAttribute, true));

            ConstrainedStringRequest actualRequest = null;

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    actualRequest = (ConstrainedStringRequest)r;
                    return new object();
                }
            };

            var sut = new MinAndMaxLengthAttributeRelay(
                new MockMemberTypeResolver());

            // Act
            sut.Create(request, context);

            //Assert
            var expectedRequest = new ConstrainedStringRequest(expectedMin, max);

            Assert.Equal(expectedRequest, actualRequest);

        }

        private class MockMemberTypeResolver : IMemberTypeResolver
        {
            public Type MemberType { get; set; } = typeof(string);

            public Type TryGetMemberType(object request)
            {
                return MemberType;
            }
        }
    }
}