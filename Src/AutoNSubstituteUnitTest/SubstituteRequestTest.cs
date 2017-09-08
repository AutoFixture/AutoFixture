using System;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteRequestTest
    {
        [Fact]
        public void TargetTypeReturnsValueSpecifiedInConstructor()
        {
            // Fixture setup
            var expectedType = typeof(IInterface);
            // Excercise system
            var sut = new SubstituteRequest(expectedType);
            // Verify outcome
            Assert.Same(expectedType, sut.TargetType);
            // Teardown
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTargetTypeIsNull()
        {
            // Fixture setup
            // Excercise system
            var e = Assert.Throws<ArgumentNullException>(() => new SubstituteRequest(null));
            // Verify outcome
            Assert.Equal("targetType", e.ParamName);
            // Teardown
        }

        [Fact]
        public void ToStringOutputContainsTypeName()
        {
            //arrange
            var type = typeof(string);
            var typeName = type.ToString();

            var sut = new SubstituteRequest(type);

            //act
            var actualOutput = sut.ToString();

            //assert
            Assert.Contains(typeName, actualOutput);
        }
    }
}
