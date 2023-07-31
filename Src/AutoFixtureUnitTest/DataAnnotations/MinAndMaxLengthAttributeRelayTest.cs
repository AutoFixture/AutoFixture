using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoFixture;
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
            var sut = new MinAndMaxLengthAttributeRelay();

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();

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
            var sut = new MinAndMaxLengthAttributeRelay();
            var dummyRequest = new object();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsNoResult()
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();
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
        public void CreateWithNonConstrainedStringRequestReturnsNoResult(object request)
        {
            // Arrange
            var sut = new MinAndMaxLengthAttributeRelay();

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
        public void CreateWithStringRequestConstrainedByMinLengthReturnsCorrectResult(int min)
        {
            // Arrange
            var minLengthAttribute = new MinLengthAttribute(min);
            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true));

            var expectedRequest = new ConstrainedStringRequest(min, min * 2);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedByMaxLengthReturnsCorrectResult(int max)
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

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        public void CreateWithStringRequestConstrainedByMinAndMaxLengthReturnsCorrectResult(int min, int max)
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

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithRangedSequenceRequestConstrainedByMinLengthReturnsCorrectResult(int min)
        {
            // Arrange
            var memberType = typeof(string[]);

            var minLengthAttribute = new MinLengthAttribute(min);
            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true));

            var expectedRangedSequenceRequest = new RangedSequenceRequest(typeof(string), min, min * 2);
            var expectedResult = new List<string>();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (expectedRangedSequenceRequest.Equals(r)) return expectedResult;
                    return new NoSpecimen();
                }
            };

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => memberType
                }
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void CreateWithRangedSequenceRequestConstrainedByMaxLengthReturnsCorrectResult(int max)
        {
            // Arrange
            var memberType = typeof(string[]);

            var maxLengthAttribute = new MaxLengthAttribute(max);
            var request = new FakeMemberInfo(
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRangedSequenceRequest = new RangedSequenceRequest(typeof(string), 1, max);
            var expectedResult = new List<string>();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (expectedRangedSequenceRequest.Equals(r)) return expectedResult;
                    return new NoSpecimen();
                }
            };

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => memberType
                }
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        public void CreateWithRangedSequenceRequestConstrainedByMinAndMaxLengthReturnsCorrectResult(int min, int max)
        {
            // Arrange
            var memberType = typeof(string[]);

            var minLengthAttribute = new MinLengthAttribute(min);
            var maxLengthAttribute = new MaxLengthAttribute(max);
            var request = new FakeMemberInfo(
                new ProvidedAttribute(minLengthAttribute, true),
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRangedSequenceRequest = new RangedSequenceRequest(typeof(string), min, max);
            var expectedResult = new List<string>();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (expectedRangedSequenceRequest.Equals(r)) return expectedResult;
                    return new NoSpecimen();
                }
            };

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => memberType
                }
            };

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

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };

            // Act
            sut.Create(request, context);

            // Assert
            var expectedRequest = new ConstrainedStringRequest(expectedMin, max);
            Assert.Equal(expectedRequest, actualRequest);
        }

        private class UnboundedMaxLengthPropertyHolder<T>
        {
            [MaxLength]
            public T Property { get; set; }
        }

        [Fact]
        public void CreateWithRangedSequenceRequestConstrainedByMaxLengthWithoutValueReturns()
        {
            // Arrange
            var memberType = typeof(string[]);

            var maxLengthAttribute = new MaxLengthAttribute();
            var request = new FakeMemberInfo(
                new ProvidedAttribute(maxLengthAttribute, true));

            var expectedRangedSequenceRequest = new RangedSequenceRequest(typeof(string), 1, 3);
            var expectedResult = new List<string>();

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRangedSequenceRequest.Equals(r)
                    ? expectedResult
                    : new NoSpecimen()
            };

            var sut = new MinAndMaxLengthAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => memberType
                }
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreatesInstanceForStringPropertyWithUnboundedMaxLengthAttribute()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var result = fixture.Create<UnboundedMaxLengthPropertyHolder<string>>();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result.Property));
        }

        [Fact]
        public void CreatesInstanceForArrayPropertyWithUnboundedMaxLengthAttribute()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var result = fixture.Create<UnboundedMaxLengthPropertyHolder<string[]>>();
            var length = result.Property.Length;

            // Assert
            Assert.True(length > 0 && length < int.MaxValue);
        }
    }
}