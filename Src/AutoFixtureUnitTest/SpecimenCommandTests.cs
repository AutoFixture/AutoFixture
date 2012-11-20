using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenCommandTests
    {
        [Fact]
        public void SingleParameterDoWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenCommand.Do<object>(null, x => { }));
        }

        [Theory]
        [InlineData(94)]
        [InlineData(1282139)]
        [InlineData(-343)]
        public void SingleParameterDoWillInvokeMethodWithCorrectParameter(
            int expected)
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => expected;

            var verified = false;
            var mock = new CommandMock<int>();
            mock.OnCommand = x => verified = expected == x;
            // Exercise system
            builder.Do((int i) => mock.Command(i));
            // Verify outcome
            Assert.True(verified, "Mock wasn't verified.");
            // Teardown
        }

        [Fact]
        public void SingleParameterDoWithNullActionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Do<object>(null));
        }

        [Fact]
        public void DoubleParameterDoWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenCommand.Do<object, object>(null, (x, y) => { }));
        }

        [Theory]
        [InlineData(3829, "ploeh")]
        [InlineData(3289, "fnaah")]
        [InlineData(3, "ndøh")]
        public void DoubleParameterDoWillInvokeMethodWithCorrectParameters(
            int expectedNumber,
            string expectedText)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(expectedNumber);
            fixture.Inject(expectedText);
            var builder = fixture.Compose();

            var verified = false;
            var mock = new CommandMock<int, string>();
            mock.OnCommand = (x, y) => verified =
                expectedNumber == x &&
                expectedText == y;
            // Exercise system
            builder.Do((int x, string y) => mock.Command(x, y));
            // Verify outcome
            Assert.True(verified, "Mock wasn't verified.");
            // Teardown
        }

        [Fact]
        public void DoubleParameterDoWithNullActionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Do<double, decimal>(null));
        }

        [Fact]
        public void TripleParameterDoWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenCommand.Do<object, object, object>(
                    null,
                    (x, y, z) => { }));
        }

        [Theory]
        [InlineData("foo", 189, false)]
        [InlineData("bar", -9, true)]
        [InlineData("baz", 0, true)]
        public void TripleParameterDoWillInvokeMethodWithCorrectParameters(
            string expectedText,
            int expectedNumber,
            bool expectedBool)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(expectedText);
            fixture.Inject(expectedNumber);
            fixture.Inject(expectedBool);
            var builder = fixture.Compose();

            var verified = false;
            var mock = new CommandMock<string, int, bool>();
            mock.OnCommand = (x, y, z) => verified =
                expectedText == x &&
                expectedNumber == y &&
                expectedBool == z;
            // Exercise system
            builder.Do((string x, int y, bool z) => mock.Command(x, y, z));
            // Verify outcome
            Assert.True(verified, "Mock wasn't verified.");
            // Teardown
        }

        [Fact]
        public void TripeParameterDoWithNullActionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Do<float, decimal, decimal>(null));
        }

        [Fact]
        public void QuadrupleParameterDoWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenCommand.Do<object, object, object, object>(
                    null,
                    (x, y, z, æ) => { }));
        }

        [Theory]
        [InlineData(428, "sgryt", typeof(Version), true)]
        [InlineData(1, "zzyxx", typeof(Stack<>), false)]
        [InlineData(-947743000, "æsel", typeof(Guid), true)]
        public void QuadrupleParameterDoWillInvokeMethodWithCorrectParameters(
            int expectedNumber,
            string expectedText,
            Type expectedType,
            bool expectedBool)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(expectedNumber);
            fixture.Inject(expectedText);
            fixture.Inject(expectedType);
            fixture.Inject(expectedBool);
            var builder = fixture.Compose();

            var verified = false;
            var mock = new CommandMock<int, string, Type, bool>();
            mock.OnCommand = (x, y, z, æ) => verified =
                expectedNumber == x &&
                expectedText == y &&
                expectedType == z &&
                expectedBool == æ;
            // Exercise system
            builder.Do(
                (int x, string y, Type z, bool æ) => mock.Command(x, y, z, æ));
            // Verify outcome
            Assert.True(verified, "Mock wasn't verified.");
            // Teardown
        }

        [Fact]
        public void QuadrupleParameterDoWithNullActionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Do<decimal, decimal, float, float>(null));
        }
    }
}
