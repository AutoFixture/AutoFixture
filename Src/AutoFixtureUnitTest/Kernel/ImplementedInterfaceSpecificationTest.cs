using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ImplementedInterfaceSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new ImplementedInterfaceSpecification(typeof(object));
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
            var sut = new ImplementedInterfaceSpecification(targetType);
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
                new ImplementedInterfaceSpecification(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new ImplementedInterfaceSpecification(typeof(object));
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForSameTypeShouldReturnTrue()
        {
            // Fixture setup
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(NoopInterfaceImplementer);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForImplementedInterfaceShouldReturnTrue()
        {
            // Fixture setup
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(IInterface);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForNotImplementedInterfaceShouldReturnFalse()
        {
            // Fixture setup
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(IDisposable);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForNonInterfaceTypeShouldReturnFalse()
        {
            // Fixture setup
            var targetType = typeof(NoopInterfaceImplementer);
            var requestedType = typeof(string);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestedType);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData("aString")]
        [InlineData(3)]
        [InlineData(false)]
        public void IsSatisfiedByWithInvalidRequestShouldReturnFalse(object request)
        {
            // Fixture setup
            var targetType = typeof(NoopInterfaceImplementer);
            var sut = new ImplementedInterfaceSpecification(targetType);
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}
