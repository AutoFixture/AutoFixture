using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Grean.Exude;
using Ploeh.Albedo;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.DataAnnotations;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;
using System.Text;
using System.Globalization;
using System.Net;

namespace Ploeh.AutoFixtureUnitTest
{
    public class IFixtureExtensionsTest
    {
        [Fact]
        public void NullIFixtureThrows()
        {
            // Fixture setup
            IFixture fixture = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => fixture.Repeat(() => new object()));
            // Teardown
        }

        [Fact]
        public void RepeatWillPerformActionTheDefaultNumberOfTimes()
        {
            // Fixture setup
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Verify outcome
            Assert.Equal<int>(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillReturnTheDefaultNumberOfItems()
        {
            // Fixture setup
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, result.Count());
            // Teardown
        }

        [Fact]
        public void RepeatWillPerformActionTheSpecifiedNumberOfTimes()
        {
            // Fixture setup
            int expectedCount = 2;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Verify outcome
            Assert.Equal<int>(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillReturnTheSpecifiedNumberOfItems()
        {
            // Fixture setup
            int expectedCount = 13;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, result.Count());
            // Teardown
        }
    }
}