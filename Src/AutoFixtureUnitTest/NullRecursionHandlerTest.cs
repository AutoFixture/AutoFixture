using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class NullRecursionHandlerTest
    {
		[Fact]
		public void CheckReturnsFalseOnUniqueType()
		{
			// Fixture setup
			var sut = new NullRecursionHandler();

			// Exercise system
			bool res = sut.Check(typeof(object));

			// Verify outcome
			Assert.False(res, "Should return false on unique type.");

			// Teardown
		}

        [Fact]
        public void CheckWithNullModeReturnsTrueOnFirstRecurrence()
        {
            // Fixture setup
            var sut = new NullRecursionHandler();
            sut.Check(typeof(object));

            // Exercise system
            bool res = sut.Check(typeof(object));

            // Verify outcome
            Assert.True(res, "Should return true on first recurrence");

            // Teardown
        }

		[Fact]
		public void GetRecursionBreakInstanceReturnsNullOnFirstRecurrence()
		{
			// Fixture setup
			var sut = new NullRecursionHandler();
			sut.Check(typeof(object));
			sut.Check(typeof(object));

			// Exercise system
			var res = sut.GetRecursionBreakInstance(typeof(object));

            Assert.Null(res);
			// Teardown
		}

        [Fact]
        public void UnCheckRemovesTypeFromCheck()
        {
            // Fixture setup
            var sut = new NullRecursionHandler();
            sut.Check(typeof(object));

            // Exercise system
            sut.Uncheck(typeof(object));
            bool res = sut.Check(typeof(object));

            // Verify outcome
            Assert.False(res, "Should return false on check of type that has been unchecked.");

            // Teardown
        }
    }
}