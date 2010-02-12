using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class SeedTest
    {
        [TestMethod]
        public void SutIsCustomAttributeProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = new Seed<object>(new object());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ICustomAttributeProvider));
            // Teardown
        }

        [TestMethod]
        public void GetCustomAttributesWillReturnInstance()
        {
            // Fixture setup
            var sut = new Seed<string>(string.Empty);
            // Exercise system
            var result = sut.GetCustomAttributes(true);
            // Verify outcome
            Assert.IsNotNull(result, "GetCustomAttributes");
            // Teardown
        }

        [TestMethod]
        public void GetSpecificCustomAttributesWillReturnInstance()
        {
            // Fixture setup
            var sut = new Seed<int>(1);
            // Exercise system
            var result = sut.GetCustomAttributes(typeof(DescriptionAttribute), false);
            // Verify outcome
            Assert.IsNotNull(result, "GetCustomAttributes");
            // Teardown
        }

        [TestMethod]
        public void IsDefinedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Seed<decimal>(-1);
            // Exercise system
            var result = sut.IsDefined(typeof(FlagsAttribute), true);
            // Verify outcome
            Assert.IsFalse(result, "IsDefined");
            // Teardown
        }

        [TestMethod]
        public void ValueIsCorrect()
        {
            // Fixture setup
            var expectedValue = "Anonymous value";
            var sut = new Seed<string>(expectedValue);
            // Exercise system
            string result = sut.Value;
            // Verify outcome
            Assert.AreEqual(expectedValue, result, "Value");
            // Teardown
        }
    }
}
