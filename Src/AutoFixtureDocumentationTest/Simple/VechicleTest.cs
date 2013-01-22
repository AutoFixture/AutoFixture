﻿using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class VechicleTest
    {
        public VechicleTest()
        {
        }

        [Fact]
        public void AnonymousVehicleHasWheelsAssignedByFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
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
                .CreateAnonymous();
            // Exercise system
            var result = sut.Wheels;
            // Verify outcome
            Assert.Equal<int>(4, result);
            // Teardown
        }
    }
}
