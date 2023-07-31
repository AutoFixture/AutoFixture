using System;
using System.Collections.Generic;
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
    public class StringLengthAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange & Act
            var sut = new StringLengthAttributeRelay();

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(null, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var request = new object();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(request, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var request = new object();
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

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
        public void CreateForUnsupportedRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var context = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithConstrainedStringRequestReturnsExpectedResult(int maximum)
        {
            // Arrange
            var stringLengthAttribute = new StringLengthAttribute(maximum);
            var providedAttribute = new ProvidedAttribute(stringLengthAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var expectedRequest = new ConstrainedStringRequest(stringLengthAttribute.MaximumLength);
            var expected = new object();
            var typeResolver = new DelegatingRequestMemberTypeResolver
            {
                OnTryGetMemberType = _ => typeof(string)
            };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expected : new NoSpecimen()
            };
            var sut = new StringLengthAttributeRelay
            {
                RequestMemberTypeResolver = typeResolver
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Same(expected, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithStringRequestConstrainedByMinimumLengthReturnsCorrectResult(int maximum)
        {
            // Arrange
            var stringLengthAttribute = new StringLengthAttribute(maximum) { MinimumLength = 1 };
            var providedAttribute = new ProvidedAttribute(stringLengthAttribute, true);
            var request = new FakeMemberInfo(providedAttribute);
            var expectedRequest = new ConstrainedStringRequest(stringLengthAttribute.MinimumLength,
                stringLengthAttribute.MaximumLength);
            var expectedResult = new object();
            var typeResolver = new DelegatingRequestMemberTypeResolver
            {
                OnTryGetMemberType = _ => typeof(string)
            };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen()
            };
            var sut = new StringLengthAttributeRelay
            {
                RequestMemberTypeResolver = typeResolver
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(GetUnsupportedValidatedMembers))]
        public void CreateForUnsupportedValidatedMembersReturnNoSpecimen(object request)
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var context = new DelegatingSpecimenContext();

            // Act
            var actual = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Theory]
        [MemberData(nameof(GetNonValidatedMembers))]
        public void CreateForNonValidatedMembersReturnNoSpecimen(object request)
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var context = new DelegatingSpecimenContext();

            // Act
            var actual = sut.Create(request, context);

            // Assert
            Assert.IsType<NoSpecimen>(actual);
        }

        [Theory]
        [MemberData(nameof(GetValidatedMembers))]
        public void CreateForValidatedMembersReturnsSpecimen(object request)
        {
            // Arrange
            var sut = new StringLengthAttributeRelay();
            var expectedRequest = new ConstrainedStringRequest(5);
            var expected = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expected : new NoSpecimen()
            };

            // Act
            var actual = sut.Create(request, context);

            // Assert
            Assert.Same(expected, actual);
        }

        [Fact]
        public void ShouldConstraintStringMembersDecoratedWithStringLengthAttribute()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var propertyHolder = fixture.Create<StringLengthValidatedPropertyHolder<string>>();
            var fieldHolder = fixture.Create<StringLengthValidatedFieldHolder<string>>();
            var constructorHost = fixture.Create<StringLengthValidatedConstructorHost<string>>();
            var actual = new[] { propertyHolder.Property, fieldHolder.Field, constructorHost.Value };

            // Assert
            Assert.All(actual, s => Assert.Equal(5, s.Length));
        }

        [Fact]
        public void ShouldNotThrowWhenResolvingDecoratedMembersOfUnsupportedTypes()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var results = new Action[]
            {
                () => _ = fixture.Create<StringLengthValidatedPropertyHolder<int>>(),
                () => _ = fixture.Create<StringLengthValidatedPropertyHolder<List<string>>>(),
                () => _ = fixture.Create<StringLengthValidatedPropertyHolder<string[]>>(),
                () => _ = fixture.Create<StringLengthValidatedFieldHolder<int>>(),
                () => _ = fixture.Create<StringLengthValidatedFieldHolder<List<string>>>(),
                () => _ = fixture.Create<StringLengthValidatedFieldHolder<string[]>>(),
                () => _ = fixture.Create<StringLengthValidatedConstructorHost<int>>(),
                () => _ = fixture.Create<StringLengthValidatedConstructorHost<List<string>>>(),
                () => _ = fixture.Create<StringLengthValidatedConstructorHost<string[]>>(),
            }.Select(Record.Exception);

            // Assert
            Assert.All(results, Assert.Null);
        }

        public static IEnumerable<object[]> GetValidatedMembers()
        {
            yield return new object[] { StringLengthValidatedPropertyHolder<string>.GetProperty() };
            yield return new object[] { StringLengthValidatedFieldHolder<string>.GetField() };
            yield return new object[] { StringLengthValidatedConstructorHost<string>.GetParameter() };
        }

        public static IEnumerable<object[]> GetUnsupportedValidatedMembers()
        {
            yield return new object[] { StringLengthValidatedPropertyHolder<int>.GetProperty() };
            yield return new object[] { StringLengthValidatedFieldHolder<int>.GetField() };
            yield return new object[] { StringLengthValidatedConstructorHost<int>.GetParameter() };
            yield return new object[] { StringLengthValidatedPropertyHolder<List<string>>.GetProperty() };
            yield return new object[] { StringLengthValidatedFieldHolder<List<string>>.GetField() };
            yield return new object[] { StringLengthValidatedConstructorHost<List<string>>.GetParameter() };
            yield return new object[] { StringLengthValidatedPropertyHolder<EnumType>.GetProperty() };
            yield return new object[] { StringLengthValidatedFieldHolder<EnumType>.GetField() };
            yield return new object[] { StringLengthValidatedConstructorHost<EnumType>.GetParameter() };
        }

        public static IEnumerable<object[]> GetNonValidatedMembers()
        {
            yield return new object[] { PropertyHolder<string>.GetProperty() };
            yield return new object[] { FieldHolder<string>.GetField() };
            yield return new object[] { UnguardedConstructorHost<string>.GetParameter() };
        }
    }
}