using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.SemanticComparison.Fluent;

namespace Ploeh.SemanticComparison.UnitTest
{
    [TestClass]
    public class LikenessSourceTest
    {
        public LikenessSourceTest()
        {
        }

        [TestMethod]
        public void ToLikenessOfNullSourceWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new LikenessSource<string>(null);
            // Exercise system
            Likeness<string, DateTime> result = sut.OfLikeness<DateTime>();
            // Verify outcome
            Assert.IsNull(result.Value, "ToLikeness");
            // Teardown
        }

        [TestMethod]
        public void ToLikenessWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedTimeSpan = TimeSpan.FromDays(1);
            var sut = new LikenessSource<TimeSpan>(expectedTimeSpan);
            // Exercise system
            var result = sut.OfLikeness<string>();
            // Verify outcome
            Assert.AreEqual(expectedTimeSpan, result.Value, "ToLikeness");
            // Teardown
        }
    }
}
