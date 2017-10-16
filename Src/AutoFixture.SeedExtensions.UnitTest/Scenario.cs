using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.SeedExtensions.UnitTest
{
    public class Scenario
    {
        [Fact]
        public void FreezeWithSeedWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze("Frozen");
            // Exercise system
            var result = sut.Create("Something else");
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWithSeedReturnsCorrectResult()
        {
            // Fixture setup
            var container = new SpecimenContext(new Fixture());
            // Exercise system
            var result = container.CreateAnonymous("Seed");
            // Verify outcome
            Assert.Contains("Seed", result);
            // Teardown
        }


        [Fact]
        [Obsolete]
        public void CreateAnonymousFromNullSpecimenContextWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateAnonymous<object>((ISpecimenContext)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Fixture setup
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix);
            // Verify outcome
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Fixture setup
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix);
            // Verify outcome
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedReturnsCorrectResult()
        {
            // Fixture setup
            var container = new Fixture();
            // Exercise system
            var result = container.CreateMany("Seed").ToList();
            // Verify outcome
            Assert.NotEmpty(result);
            Assert.True(result.All(s => s.Contains("Seed")));
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 73;
            var sut = new Fixture();
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 29;
            var sut = new Fixture();
            sut.RepeatCount = expectedItemCount;
            // Exercise system
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }


        [Fact]
        public void CreateManyWithSeedAndCountReturnsCorrectResult()
        {
            // Fixture setup
            var seed = "Seed";
            var count = 2;
            var container = new Fixture();
            // Exercise system
            var result = container.CreateMany(seed, count).ToList();
            // Verify outcome
            Assert.Equal(count, result.Count);
            Assert.True(result.All(s => s.Contains(seed)));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStringWillPrefixName()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.Create(expectedText);
            // Verify outcome
            string actualText = new TextGuidRegex().GetText(result);
            Assert.Equal(expectedText, actualText);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStringWillAppendGuid()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.Create(anonymousText);
            // Verify outcome
            string guidString = new TextGuidRegex().GetGuid(result);
            Guid g = new Guid(guidString);
            Assert.NotEqual<Guid>(Guid.Empty, g);
            // Teardown
        }


        [Fact]
        public void CustomizeWithEchoInt32GeneratorWillReturnSeed()
        {
            // Fixture setup
            int expectedValue = 4;
            Fixture sut = new Fixture();
            sut.Customize<int>(c => c.FromSeed(s => s));
            // Exercise system
            int result = sut.Create(expectedValue);
            // Verify outcome
            Assert.Equal<int>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void BuildFromSeedWillCreateUsingCorrectSeed()
        {
            // Fixture setup
            var sut = new Fixture();
            var seed = new object();

            var verified = false;
            Func<object, object> mock = s => verified = seed.Equals(s);
            // Exercise system
            sut.Build<object>().FromSeed(mock).Create(seed);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }


        [Fact]
        public void CreateWithSeedReturnsCorrectResult()
        {
            // Fixture setup
            var container = new Fixture();
            // Exercise system
            var result = container.Create("Seed");
            // Verify outcome
            Assert.Contains("Seed", result);
            // Teardown
        }
    }
}
