using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UnwrapMemberRequestTest
    {
        [Fact]
        public void ShouldThrowIfNullInnerBuilderIsPassedToConstructor()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UnwrapMemberRequest(builder: null));
        }

        [Fact]
        public void ShouldSaveThePassedBuilderToProperty()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();

            // Act
            var sut = new UnwrapMemberRequest(builder);

            // Assert
            Assert.Same(builder, sut.Builder);
        }

        [Fact]
        public void DefaultResolverShouldHaveCorrectType()
        {
            // Act
            var sut = new UnwrapMemberRequest(new DelegatingSpecimenBuilder());

            // Assert
            Assert.IsType<RequestMemberTypeResolver>(sut.MemberTypeResolver);
        }

        [Fact]
        public void ShouldThrowIfNullMemberTypeResolverIsAssigned()
        {
            // Arrange
            var sut = new UnwrapMemberRequest(new DelegatingSpecimenBuilder());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.MemberTypeResolver = null);
        }

        [Fact]
        public void ShouldPassUnwrappedTypeToInnerBuilder()
        {
            // Arrange
            var memberRequest = new object();
            var memberType = typeof(List<int>);

            var memberTypeResolver = new DelegatingRequestMemberTypeResolver
            {
                OnTryGetMemberType = r => r == memberRequest ? memberType : null
            };

            var expectedResult = new object();
            var innerBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => memberType.Equals(r) ? expectedResult : new NoSpecimen()
            };

            var sut = new UnwrapMemberRequest(innerBuilder)
            {
                MemberTypeResolver = memberTypeResolver
            };

            var ctx = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(memberRequest, ctx);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ShouldNotInvokeInnerBuilderIfUnableToResolveMemberType()
        {
            // Arrange
            var nonMemberRequest = new object();
            var memberTypeResolver = new DelegatingRequestMemberTypeResolver
            {
                OnTryGetMemberType = _ => null
            };

            var innerBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => throw new InvalidOperationException("Should not be invoked")
            };

            var sut = new UnwrapMemberRequest(innerBuilder)
            {
                MemberTypeResolver = memberTypeResolver
            };

            var ctx = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(nonMemberRequest, ctx);

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }
    }
}