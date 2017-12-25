using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Simple
{
    public class VehicleTest
    {
        public VehicleTest()
        {
        }

        [Fact]
        public void AnonymousVehicleHasWheelsAssignedByFixture()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(5, byte.MaxValue));

            var sut = fixture.Create<Vehicle>();
            // Act
            var result = sut.Wheels;
            // Assert
            Assert.NotEqual<int>(4, result);
        }

        [Fact]
        public void VehicleWithoutAutoPropertiesWillHaveFourWheels()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<Vehicle>()
                .OmitAutoProperties()
                .Create();
            // Act
            var result = sut.Wheels;
            // Assert
            Assert.Equal<int>(4, result);
        }
    }
}
