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
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Repeat(() => new object()));
            // Teardown
        }
    }
}
