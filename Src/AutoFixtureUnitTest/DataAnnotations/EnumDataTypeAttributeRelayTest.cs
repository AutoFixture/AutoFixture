using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class EnumDataTypeAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            // Act & assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
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
        public void CreateWithNonEnumDataTypeAttributeRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(TriState), 1, TriState.First)]
        [InlineData(typeof(TriState), 2, TriState.Second)]
        [InlineData(typeof(TriState), 3, TriState.Third)]
        [InlineData(typeof(TriState), 4, TriState.First)]
        [InlineData(typeof(TriState), 5, TriState.Second)]
        [InlineData(typeof(DayOfWeek), 1, DayOfWeek.Sunday)]
        [InlineData(typeof(DayOfWeek), 2, DayOfWeek.Monday)]
        [InlineData(typeof(DayOfWeek), 8, DayOfWeek.Sunday)]
        public void RequestForEnumTypeReturnsCorrectResult(Type enumType, int requestCount, object expectedResult)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Repeat<Func<object>>(() => sut.Create(request, dummyContext), requestCount).Select(f => f()).ToArray().Last();
            // Assert
            Assert.Equal(expectedResult.ToString(), result);
        }

        [Fact]
        public void RequestForEnumWithNoValuesThrows()
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(typeof(EmptyEnum));
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };
            // Act & assert
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ObjectCreationException>(() => sut.Create(request, dummyContext));
        }
    }
}
