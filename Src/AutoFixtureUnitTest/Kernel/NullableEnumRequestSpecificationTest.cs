using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NullableEnumRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullableEnumRequestSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(TriState))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(PropertyHolder<int>))]
        [InlineData(typeof(DoubleFieldHolder<int, string>))]
        public void IsSatisfiedReturnsFalseOnRequestWhichIsNotRequestForNullableEnum(object request)
        {
            // Fixture setup
            var sut = new NullableEnumRequestSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TriState?))]
        [InlineData(typeof(ConsoleColor?))]
        public void IsSatisfiedReturnsTrueForRequestForNullableEnum(object request)
        {
            // Fixture setup
            var sut = new NullableEnumRequestSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}
