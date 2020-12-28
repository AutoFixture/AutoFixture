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
        public void CreateWithNullRequestReturnsCorrectResult()
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
        public void CreateWithAnonymousRequestReturnsCorrectResult()
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
        public void CreateWithNonEnumDataTypeAttributeRequestReturnsCorrectResult(object request)
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
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };
            // Act
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
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new EnumDataTypeAttributeRelay
            {
                RequestMemberTypeResolver = new DelegatingRequestMemberTypeResolver
                {
                    OnTryGetMemberType = r => typeof(string)
                }
            };
            // Act & assert
            Assert.Throws<ObjectCreationException>(() => sut.Create(request, dummyContext));
        }

        [Theory]
        [InlineData(typeof(TriState), TriState.Third)]
        [InlineData(typeof(DayOfWeek), DayOfWeek.Monday)]
        [InlineData(typeof(ConsoleColor), ConsoleColor.Black)]
        public void SutCanCorrectlyInterleaveDifferentEnumTypes(Type enumType, object expectedResult)
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
                    OnTryGetMemberType = r => typeof(string)
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
            Assert.Equal(expectedResult.ToString(), result);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), typeof(byte))]
        [InlineData(typeof(SbyteEnumType), typeof(sbyte))]
        [InlineData(typeof(ShortEnumType), typeof(short))]
        [InlineData(typeof(UshortEnumType), typeof(ushort))]
        [InlineData(typeof(IntEnumType), typeof(int))]
        [InlineData(typeof(UintEnumType), typeof(uint))]
        [InlineData(typeof(LongEnumType), typeof(long))]
        [InlineData(typeof(UlongEnumType), typeof(ulong))]
        public void RequestReturnsCorrectTypeForDifferentMemberTypes(Type enumType, Type memberType)
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
                    OnTryGetMemberType = r => memberType
                }
            };
            // Act
            var result = sut.Create(request, dummyContext);
            // Assert
            Assert.IsType(memberType, result);
        }

        [Theory]
        [InlineData(typeof(ByteEnumType), typeof(byte), 2, (byte)ByteEnumType.Second)]
        [InlineData(typeof(SbyteEnumType), typeof(sbyte), 1, (sbyte)SbyteEnumType.First)]
        [InlineData(typeof(ShortEnumType), typeof(short), 3, (short)ShortEnumType.Third)]
        [InlineData(typeof(UshortEnumType), typeof(ushort), 4, (ushort)UshortEnumType.First)]
        [InlineData(typeof(IntEnumType), typeof(int), 5, (int)IntEnumType.Second)]
        [InlineData(typeof(UintEnumType), typeof(uint), 2, (uint)UintEnumType.Second)]
        [InlineData(typeof(LongEnumType), typeof(long), 1, (long)LongEnumType.First)]
        [InlineData(typeof(UlongEnumType), typeof(ulong), 8, (ulong)UlongEnumType.Second)]
        public void RequestReturnsCorrectResultForDifferentEnumTypes(Type enumType, Type memberType, int requestCount, object expectedResult)
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
                    OnTryGetMemberType = r => memberType
                }
            };
            // Act
            var result = Enumerable.Repeat<Func<object>>(() => sut.Create(request, dummyContext), requestCount).Select(f => f()).ToArray().Last();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        private enum ByteEnumType : byte
        {
            First = 99,
            Second,
            Third
        }

        private enum SbyteEnumType : sbyte
        {
            First = -98,
            Second,
            Third
        }

        private enum ShortEnumType : short
        {
            First = 100,
            Second,
            Third
        }

        private enum UshortEnumType : ushort
        {
            First = 1,
            Second,
            Third
        }

        private enum IntEnumType : int
        {
            First = 7,
            Second,
            Third
        }

        private enum UintEnumType : uint
        {
            First,
            Second,
            Third
        }

        private enum LongEnumType : long
        {
            First = (long)int.MaxValue + 1,
            Second,
            Third
        }

        private enum UlongEnumType : ulong
        {
            First = 1,
            Second,
            Third
        }
    }
}
