using System;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RangeAttributeOnDecimalHandlingTest
    {
        [Fact]
        public void CreateAnonymousDoesNotThrowForStringAttribute()
        {
            var sut = new Fixture();
            Assert.DoesNotThrow(() => sut.CreateAnonymous<FooString>());
        }

        [Fact]
        public void CreateAnonymousDoesNotThrowForIntAttribute()
        {
            var sut = new Fixture();
            Assert.DoesNotThrow(() => sut.CreateAnonymous<FooInt>());
        }

        [Fact]
        public void CreateAnonymousDoesNotThrowForDoubleAttribute()
        {
            var sut = new Fixture();
            Assert.DoesNotThrow(() => sut.CreateAnonymous<FooDouble>());
        }

        public class FooString
        {
            [Range(typeof(Decimal), "0", "100")]
            public decimal Bar1 { get; set; }

            [Range(typeof(Decimal), "0", "100")]
            public decimal Bar2 { get; set; }
        }

        public class FooInt
        {
            [Range(0, 100)]
            public decimal Bar1 { get; set; }

            [Range(0, 100)]
            public decimal Bar2 { get; set; }
        }

        public class FooDouble
        {
            [Range(0D, 100D)]
            public decimal Bar1 { get; set; }

            [Range(0D, 100D)]
            public decimal Bar2 { get; set; }
        }
    }
}
