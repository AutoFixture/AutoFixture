using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ExactTypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system
            var sut = new ExactTypeSpecification(dummyType);
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new ExactTypeSpecification(null));
            // Teardown
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Fixture setup
            var expectedType = typeof(DayOfWeek);
            var sut = new ExactTypeSpecification(expectedType);
            // Exercise system
            Type result = sut.TargetType;
            // Verify outcome
            Assert.Equal(expectedType, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var sut = new ExactTypeSpecification(dummyType);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(string), typeof(int), false)]
        [InlineData(typeof(PropertyHolder<string>), typeof(FieldHolder<string>), false)]
        public void IsSatisfiedByReturnsCorrectResult(Type specType, Type requestType, bool expectedResult)
        {
            // Fixture setup
            var sut = new ExactTypeSpecification(specType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestType);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
