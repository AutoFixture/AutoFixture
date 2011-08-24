using System;
using Ploeh.SemanticComparison.Fluent;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class LikenessSourceTest
    {
        [Fact]
        public void ToLikenessOfNullSourceWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new LikenessSource<string>(null);
            // Exercise system
            Likeness<string, DateTime> result = sut.OfLikeness<DateTime>();
            // Verify outcome
            Assert.Null(result.Value);
            // Teardown
        }

        [Fact]
        public void ToLikenessWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedTimeSpan = TimeSpan.FromDays(1);
            var sut = new LikenessSource<TimeSpan>(expectedTimeSpan);
            // Exercise system
            var result = sut.OfLikeness<string>();
            // Verify outcome
            Assert.Equal(expectedTimeSpan, result.Value);
            // Teardown
        }
    }
}
