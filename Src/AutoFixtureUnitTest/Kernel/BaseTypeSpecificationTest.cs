using System;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    using AutoFixture.Kernel;
    using TestTypeFoundation;
    using Xunit.Extensions;

    public class BaseTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new BaseTypeSpecification(typeof(ConcreteType));
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
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
            var sut = new BaseTypeSpecification(typeof(ConcreteType));
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForCompatibleTypeShouldReturnTrue()
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
