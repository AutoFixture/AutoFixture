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
	public class ThrowingRecursionHandlerTest
	{
		[TestMethod]
		public void CheckReturnsFalseOnUniqueType()
		{
			// Fixture setup
			var sut = new ThrowingRecursionHandler();

			// Exercise system
			bool res = sut.Check(typeof(object));

			// Verify outcome
			Assert.IsFalse(res, "Should return false on unique type.");

			// Teardown
		}

		[TestMethod]
		public void CheckReturnsTrueOnFirstRecurrence()
		{
			// Fixture setup
			var sut = new ThrowingRecursionHandler();
			sut.Check(typeof(object));

			// Exercise system
			bool res = sut.Check(typeof(object));

			// Verify outcome
			Assert.IsTrue(res, "Should return true on first recurrence.");

			// Teardown
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectCreationException))]
		public void GetRecursionBreakInstanceThrowsOnFirstRecurrence()
		{
			// Fixture setup
			var sut = new ThrowingRecursionHandler();
			sut.Check(typeof(object));
			sut.Check(typeof(object));

			// Exercise system
			sut.GetRecursionBreakInstance(typeof(object));

			// Teardown
		}

		[TestMethod]
		public void UnCheckRemovesTypeFromCheck()
		{
			// Fixture setup
			var sut = new ThrowingRecursionHandler();
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