using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.SeedExtensions.UnitTest
{
    public class Scenario
    {
        [Fact]
        public void FreezeWithSeedWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Arrange
            var sut = new Fixture();
            var expectedResult = sut.Freeze("Frozen");
            // Act
            var result = sut.Create("Something else");
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWithSeedReturnsCorrectResult()
        {
            // Arrange
            var container = new SpecimenContext(new Fixture());
            // Act
            var result = container.CreateAnonymous("Seed");
            // Assert
            Assert.Contains("Seed", result);
        }


        [Fact]
        [Obsolete]
        public void CreateAnonymousFromNullSpecimenContextWithSeedThrows()
        {
            // Arrange
            var dummySeed = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateAnonymous<object>((ISpecimenContext)null, dummySeed));
        }

        [Fact]
        public void CreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Arrange
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Act
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix);
            // Assert
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
        }

        [Fact]
        public void BuildAndCreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Arrange
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Act
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix);
            // Assert
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
        }

        [Fact]
        public void CreateManyWithSeedReturnsCorrectResult()
        {
            // Arrange
            var container = new Fixture();
            // Act
            var result = container.CreateMany("Seed").ToList();
            // Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(s => s.Contains("Seed")));
        }

        [Fact]
        public void CreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Arrange
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 73;
            var sut = new Fixture();
            // Act
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix, expectedItemCount);
            // Assert
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
        }

        [Fact]
        public void BuildAndCreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Arrange
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 29;
            var sut = new Fixture();
            sut.RepeatCount = expectedItemCount;
            // Act
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix, expectedItemCount);
            // Assert
            int actualCount = (from s in result
                where s.StartsWith(anonymousPrefix)
                select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
        }


        [Fact]
        public void CreateManyWithSeedAndCountReturnsCorrectResult()
        {
            // Arrange
            var seed = "Seed";
            var count = 2;
            var container = new Fixture();
            // Act
            var result = container.CreateMany(seed, count).ToList();
            // Assert
            Assert.Equal(count, result.Count);
            Assert.True(result.All(s => s.Contains(seed)));
        }

        [Fact]
        public void CreateAnonymousStringWillPrefixName()
        {
            // Arrange
            string expectedText = "Anonymous text";
            Fixture sut = new Fixture();
            // Act
            string result = sut.Create(expectedText);
            // Assert
            string actualText = new TextGuidRegex().GetText(result);
            Assert.Equal(expectedText, actualText);
        }

        [Fact]
        public void CreateAnonymousStringWillAppendGuid()
        {
            // Arrange
            string anonymousText = "Anonymous text";
            Fixture sut = new Fixture();
            // Act
            string result = sut.Create(anonymousText);
            // Assert
            string guidString = new TextGuidRegex().GetGuid(result);
            Guid g = new Guid(guidString);
            Assert.NotEqual<Guid>(Guid.Empty, g);
        }


        [Fact]
        public void CustomizeWithEchoInt32GeneratorWillReturnSeed()
        {
            // Arrange
            int expectedValue = 4;
            Fixture sut = new Fixture();
            sut.Customize<int>(c => c.FromSeed(s => s));
            // Act
            int result = sut.Create(expectedValue);
            // Assert
            Assert.Equal<int>(expectedValue, result);
        }

        [Fact]
        public void BuildFromSeedWillCreateUsingCorrectSeed()
        {
            // Arrange
            var sut = new Fixture();
            var seed = new object();

            var verified = false;
            Func<object, object> mock = s => verified = seed.Equals(s);
            // Act
            sut.Build<object>().FromSeed(mock).Create(seed);
            // Assert
            Assert.True(verified, "Mock verified");
        }


        [Fact]
        public void CreateWithSeedReturnsCorrectResult()
        {
            // Arrange
            var container = new Fixture();
            // Act
            var result = container.Create("Seed");
            // Assert
            Assert.Contains("Seed", result);
        }
    }
}
