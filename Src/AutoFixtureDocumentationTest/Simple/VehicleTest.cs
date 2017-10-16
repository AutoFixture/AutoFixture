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
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(5, byte.MaxValue));

            var sut = fixture.Create<Vehicle>();
            // Exercise system
            var result = sut.Wheels;
            // Verify outcome
            Assert.NotEqual<int>(4, result);
            // Teardown
        }

        [Fact]
        public void VehicleWithoutAutoPropertiesWillHaveFourWheels()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<Vehicle>()
                .OmitAutoProperties()
                .Create();
            // Exercise system
            var result = sut.Wheels;
            // Verify outcome
            Assert.Equal<int>(4, result);
            // Teardown
        }
    }
}
