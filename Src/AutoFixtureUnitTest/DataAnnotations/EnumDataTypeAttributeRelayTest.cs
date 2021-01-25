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
        public void ThrowsForNullArguments()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new EnumDataTypeAttributeRelay(null));
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(null, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(dummyRequest, dummyContainer);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void CreateWithUnsupportedRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var sut = new EnumDataTypeAttributeRelay();
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Theory]
        [InlineData(typeof(TriState), 1, nameof(TriState.First))]
        [InlineData(typeof(TriState), 2, nameof(TriState.Second))]
        [InlineData(typeof(TriState), 3, nameof(TriState.Third))]
        [InlineData(typeof(TriState), 4, nameof(TriState.First))]
        [InlineData(typeof(TriState), 5, nameof(TriState.Second))]
        [InlineData(typeof(DayOfWeek), 1, nameof(DayOfWeek.Sunday))]
        [InlineData(typeof(DayOfWeek), 2, nameof(DayOfWeek.Monday))]
        [InlineData(typeof(DayOfWeek), 8, nameof(DayOfWeek.Sunday))]
        public void RequestForEnumTypeReturnsResultAsString(
            Type enumType, int requestCount, string expectedResult)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => typeof(string)
                }
            };

            // Act
            var result = Enumerable
                .Repeat<Func<object>>(() => sut.Create(request, dummyContext), requestCount)
                .Select(f => f()).ToArray().Last();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void RequestForEnumWithNoValuesThrows()
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(typeof(EmptyEnum));
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => typeof(string)
                }
            };

            // Act & assert
            Assert.Throws<ObjectCreationException>(() => sut.Create(request, dummyContext));
        }

        [Theory]
        [InlineData(typeof(TriState), nameof(TriState.Third))]
        [InlineData(typeof(DayOfWeek), nameof(DayOfWeek.Monday))]
        [InlineData(typeof(ConsoleColor), nameof(ConsoleColor.Black))]
        public void CanCorrectlyInterleaveDifferentEnumTypes(Type enumType, string expectedResult)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => typeof(string)
                }
            };

            var triStateEnumDataTypeAttribute = new EnumDataTypeAttribute(typeof(TriState));
            var triStateProvidedAttribute = new ProvidedAttribute(triStateEnumDataTypeAttribute, true);
            var dayOfWeekEnumDataTypeAttribute = new EnumDataTypeAttribute(typeof(DayOfWeek));
            var dayOfWeekProvidedAttribute = new ProvidedAttribute(dayOfWeekEnumDataTypeAttribute, true);

            sut.Create(new FakeMemberInfo(triStateProvidedAttribute), dummyContext);
            sut.Create(new FakeMemberInfo(triStateProvidedAttribute), dummyContext);
            sut.Create(new FakeMemberInfo(dayOfWeekProvidedAttribute), dummyContext);

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), typeof(byte))]
        [InlineData(typeof(SByteEnumType), typeof(sbyte))]
        [InlineData(typeof(ShortEnumType), typeof(short))]
        [InlineData(typeof(UShortEnumType), typeof(ushort))]
        [InlineData(typeof(EnumType), typeof(int))]
        [InlineData(typeof(UIntEnumType), typeof(uint))]
        [InlineData(typeof(LongEnumType), typeof(long))]
        [InlineData(typeof(ULongEnumType), typeof(ulong))]
        public void RequestReturnsCorrectTypeForDifferentMemberTypes(
            Type enumType, Type memberType)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => memberType
                }
            };

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType(memberType, result);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), typeof(byte), 2, (byte)ByteEnumType.Second)]
        [InlineData(typeof(SByteEnumType), typeof(sbyte), 1, (sbyte)SByteEnumType.First)]
        [InlineData(typeof(ShortEnumType), typeof(short), 3, (short)ShortEnumType.Third)]
        [InlineData(typeof(UShortEnumType), typeof(ushort), 4, (ushort)UShortEnumType.First)]
        [InlineData(typeof(EnumType), typeof(int), 5, (int)EnumType.Second)]
        [InlineData(typeof(UIntEnumType), typeof(uint), 2, (uint)UIntEnumType.Second)]
        [InlineData(typeof(LongEnumType), typeof(long), 1, (long)LongEnumType.First)]
        [InlineData(typeof(ULongEnumType), typeof(ulong), 8, (ulong)ULongEnumType.Second)]
        public void RequestReturnsCorrectResultForDifferentEnumTypes(
            Type enumType, Type memberType, int requestCount, object expectedResult)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => memberType
                }
            };

            // Act
            var result = Enumerable
                .Repeat<Func<object>>(() => sut.Create(request, dummyContext), requestCount)
                .Select(f => f()).ToArray().Last();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), ByteEnumType.First)]
        [InlineData(typeof(SByteEnumType), SByteEnumType.First)]
        [InlineData(typeof(ShortEnumType), ShortEnumType.First)]
        [InlineData(typeof(UShortEnumType), UShortEnumType.First)]
        [InlineData(typeof(EnumType), EnumType.First)]
        [InlineData(typeof(UIntEnumType), UIntEnumType.First)]
        [InlineData(typeof(LongEnumType), LongEnumType.First)]
        [InlineData(typeof(ULongEnumType), ULongEnumType.First)]
        public void RequestReturnsExpectedResultForObjectMemberType(Type enumType, object expectedResult)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => typeof(object)
                }
            };

            // Act
            var actual = sut.Create(request, dummyContext);

            // Assert
            Assert.Equal(expectedResult, actual);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), typeof(ByteEnumType), ByteEnumType.First)]
        [InlineData(typeof(SByteEnumType), typeof(SByteEnumType), SByteEnumType.First)]
        [InlineData(typeof(ShortEnumType), typeof(ShortEnumType), ShortEnumType.First)]
        [InlineData(typeof(UShortEnumType), typeof(UShortEnumType), UShortEnumType.First)]
        [InlineData(typeof(EnumType), typeof(EnumType), EnumType.First)]
        [InlineData(typeof(UIntEnumType), typeof(UIntEnumType), UIntEnumType.First)]
        [InlineData(typeof(LongEnumType), typeof(LongEnumType), LongEnumType.First)]
        [InlineData(typeof(ULongEnumType), typeof(ULongEnumType), ULongEnumType.First)]
        public void RequestReturnsExpectedResultForEnumMemberType(Type enumType, Type memberType, object expectedResult)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => memberType
                }
            };

            // Act
            var actual = sut.Create(request, dummyContext);

            // Assert
            Assert.Equal(expectedResult, actual);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType))]
        [InlineData(typeof(SByteEnumType))]
        [InlineData(typeof(ShortEnumType))]
        [InlineData(typeof(UShortEnumType))]
        [InlineData(typeof(EnumType))]
        [InlineData(typeof(UIntEnumType))]
        [InlineData(typeof(LongEnumType))]
        [InlineData(typeof(ULongEnumType))]
        public void ReturnsNoSpecimenWhenEnumGeneratorReturnsNoSpecimen(Type enumType)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var enumGenerator = new DelegatingSpecimenBuilder
            {
                OnCreate = (_, _) => new NoSpecimen()
            };
            var sut = new EnumDataTypeAttributeRelay(enumGenerator);

            // Act
            var actual = sut.Create(request, null);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType))]
        [InlineData(typeof(SByteEnumType))]
        [InlineData(typeof(ShortEnumType))]
        [InlineData(typeof(UShortEnumType))]
        [InlineData(typeof(EnumType))]
        [InlineData(typeof(UIntEnumType))]
        [InlineData(typeof(LongEnumType))]
        [InlineData(typeof(ULongEnumType))]
        public void ReturnsNoSpecimenWhenResolverReturnsNoSpecimen(Type enumType)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => null
                }
            };

            // Act
            var actual = sut.Create(request, null);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), typeof(PropertyHolder<string>))]
        [InlineData(typeof(SByteEnumType), typeof(PropertyHolder<int>))]
        [InlineData(typeof(ShortEnumType), typeof(PropertyHolder<long>))]
        [InlineData(typeof(UShortEnumType), typeof(PropertyHolder<object>))]
        [InlineData(typeof(EnumType), typeof(PropertyHolder<float>))]
        [InlineData(typeof(UIntEnumType), typeof(PropertyHolder<decimal>))]
        [InlineData(typeof(LongEnumType), typeof(PropertyHolder<double>))]
        [InlineData(typeof(ULongEnumType), typeof(PropertyHolder<long>))]
        public void ReturnsNoSpecimenResultForCustomClassMemberType(Type enumType, Type memberType)
        {
            // Arrange
            var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);
            var providedAttribute = new ProvidedAttribute(enumDataTypeAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = _ => memberType
                }
            };

            // Act
            var actual = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }
    }
}
