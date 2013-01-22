using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenQueryTests
    {
        [Fact]
        public void SingleParameterGetWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenQuery.Get<object, object>(null, x => x));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void SingleParameterGetReturnsCorrectResult(int number)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(number);
            // Exercise system
            var actual = fixture.Get((int x) => -1 * x);
            // Verify outcome
            Assert.Equal(-1 * number, actual);
            // Teardown
        }

        [Fact]
        public void SingleParameterGetWithNullFunctionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Get<object, float>(null));
        }

        [Fact]
        public void DoubleParameterGetWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenQuery.Get<object, object, object>(null, (x, y) => x));
        }

        [Theory]
        [InlineData(1, "ploeh")]
        [InlineData(2, "fnaah")]
        [InlineData(3, "ndøh")]
        public void DoubleParameterGetReturnsCorrectResult(
            int number,
            string text)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(number);
            fixture.Inject(text);
            // Exercise system
            var actual = fixture.Get((int x, string y) => x + y);
            // Verify outcome
            Assert.Equal(number + text, actual);
            // Teardown
        }

        [Fact]
        public void DoubleParameterGetWithNullFuctionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Get<float, float, short>(null));
        }

        [Fact]
        public void TripleParameterGetWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenQuery.Get<object, object, object, object>(
                    null,
                    (x, y, z) => x));
        }

        [Theory]
        [InlineData("foo", false, 17)]
        [InlineData("bar", true, -9)]
        [InlineData("qux", true, 100)]
        public void TripleParameterGetReturnsCorrectResult(
            string text,
            bool logical,
            long number)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(text);
            fixture.Inject(logical);
            fixture.Inject(number);
            // Exercise system
            var actual = fixture.Get((string x, bool y, long z) => x + y + z);
            // Verify outcome
            Assert.Equal(text + logical + number, actual);
            // Teardown
        }

        [Fact]
        public void TripleParameterGetWithNullFunctionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Get<float, Type, Version, string>(null));
        }

        [Fact]
        public void QuadrupleParameterGetWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenQuery.Get<object, object, object, object, object>(
                    null,
                    (x, y, z, æ) => x));
        }

        [Theory]
        [InlineData(typeof(Version), true, 89, "hål")]
        [InlineData(typeof(Encoder), false, 321, "gnyf")]
        [InlineData(typeof(object), false, 101101, "Urt")]
        public void QuadrupleParameterGetReturnsCorrectResult(
            Type type,
            bool logical,
            int number,
            string text)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(type);
            fixture.Inject(logical);
            fixture.Inject(number);
            fixture.Inject(text);
            // Exercise system
            var actual = fixture.Get((Type x, bool y, int z, string æ) =>
                x.ToString() + y + z + æ);
            // Verify outcome
            Assert.Equal(
                type.ToString() + logical + number + text,
                actual);
            // Teardown
        }

        [Fact]
        public void QuadrupleParameterGetWithNullFunctionThrows()
        {
            var builder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.Get<object, object, short, Type, Version>(null));
        }
    }
}
