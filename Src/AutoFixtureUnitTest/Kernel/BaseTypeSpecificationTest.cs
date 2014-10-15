using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class BaseTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new BaseTypeSpecification(typeof(object));
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var targetType = typeof(object);
            // Exercise system
            var sut = new BaseTypeSpecification(targetType);
            // Verify outcome
            Assert.Equal(targetType, sut.TargetType);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTargetTypeShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BaseTypeSpecification(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new BaseTypeSpecification(typeof(object));
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForDirectBaseTypeShouldReturnTrue()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(AbstractType);
            var sut = new BaseTypeSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForSameTypeShouldReturnTrue()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(ConcreteType);
            var sut = new BaseTypeSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForIndirectBaseTypeShouldReturnFalse()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(object);
            var sut = new BaseTypeSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForIncompatibleTypeShouldReturnFalse()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
            var requestedType = typeof(string);
            var sut = new BaseTypeSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData("aString")]
        [InlineData(9)]
        [InlineData(true)]
        public void IsSatisfiedByWithInvalidRequestShouldReturnFalse(object request)
        {
            // Fixture setup
            var sut = new BaseTypeSpecification(typeof(ConcreteType));
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}
