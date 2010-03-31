using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class NullRecursionHandlerTest
    {
		[TestMethod]
		public void CheckReturnsFalseOnUniqueType()
		{
			// Fixture setup
			var sut = new NullRecursionHandler();

			// Exercise system
			bool res = sut.Check(typeof(object));

			// Verify outcome
			Assert.IsFalse(res, "Should return false on unique type.");

			// Teardown
		}

        [TestMethod]
        public void CheckWithNullModeReturnsTrueOnFirstRecurrence()
        {
            // Fixture setup
            var sut = new NullRecursionHandler();
            sut.Check(typeof(object));

            // Exercise system
            bool res = sut.Check(typeof(object));

            // Verify outcome
            Assert.IsTrue(res, "Should return true on first recurrence");

            // Teardown
        }

		[TestMethod]
		public void GetRecursionBreakInstanceReturnsNullOnFirstRecurrence()
		{
			// Fixture setup
			var sut = new NullRecursionHandler();
			sut.Check(typeof(object));
			sut.Check(typeof(object));

			// Exercise system
			var res = sut.GetRecursionBreakInstance(typeof(object));

            Assert.IsNull(res, "Should return null on first recurrence.");
			// Teardown
		}

        [TestMethod]
        public void UnCheckRemovesTypeFromCheck()
        {
            // Fixture setup
            var sut = new NullRecursionHandler();
            sut.Check(typeof(object));

            // Exercise system
            sut.Uncheck(typeof(object));
            bool res = sut.Check(typeof(object));

            // Verify outcome
            Assert.IsFalse(res, "Should return false on check of type that has been unchecked.");

            // Teardown
        }
    }
}