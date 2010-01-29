using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    [TestClass]
    public class ConstrainedStringGeneratorTest
    {
        public ConstrainedStringGeneratorTest()
        {
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void CreateWithMaxLessThanZeroWillThrow()
        {
            // Fixture setup
            // Exercise system
            new ConstrainedStringGenerator(-187, -1);
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void CreateWithMinLargerThanMaxWillThrow()
        {
            // Fixture setup
            // Exercise system
            new ConstrainedStringGenerator(9, 8);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillPrefixWithSeed()
        {
            // Fixture setup
            var seed = "Anonymous seed";
            var sut = new ConstrainedStringGenerator(1, 100);
            // Exercise system
            var result = sut.CreateaAnonymous(seed);
            // Verify outcome
            StringAssert.StartsWith(result, seed, "Result should start with seed.");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillSatisfyMinimumWhenLow()
        {
            // Fixture setup
            var minimum = 4;
            var sut = new ConstrainedStringGenerator(minimum, 100);
            // Exercise system
            string result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.IsTrue(result.Length >= minimum, "Length greater than minimum");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillSatisfyMinimumWhenHigh()
        {
            // Fixture setup
            var minimum = 109;
            var sut = new ConstrainedStringGenerator(minimum, 200);
            // Exercise system
            string result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.IsTrue(result.Length >= minimum, "Length greater than minimum");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillSatisfyMaximumWhenLow()
        {
            // Fixture setup
            var maximum = 10;
            var sut = new ConstrainedStringGenerator(1, maximum);
            // Exercise system
            var result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.IsTrue(result.Length <= maximum, "Length less than maximum");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillSatisfyMaxiumWhenHigh()
        {
            // Fixture setup
            var maximum = 89;
            var sut = new ConstrainedStringGenerator(1, maximum);
            // Exercise system
            var result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.IsTrue(result.Length <= maximum, "Length less than maximum");
            // Teardown
        }
    }
}
