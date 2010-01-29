using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    [TestClass]
    public class VechicleTest
    {
        public VechicleTest()
        {
        }

        [ExpectedException(typeof(AssertFailedException))]
        [TestMethod]
        public void AnonymousVehicleHasWheelsAssignedByFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Vehicle>();
            // Exercise system
            var result = sut.Wheels;
            // Verify outcome
            Assert.AreEqual<int>(4, result, "Wheels");
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<int>(4, result, "Wheels");
            // Teardown
        }
    }
}
