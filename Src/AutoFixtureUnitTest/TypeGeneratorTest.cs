using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TypeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new TypeGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData("")]
        [InlineData(false)]
        [InlineData(true)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateNonTypeReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new TypeGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void CreateTypeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new TypeGenerator();
            var request = typeof(Type);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = typeof(object);
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
