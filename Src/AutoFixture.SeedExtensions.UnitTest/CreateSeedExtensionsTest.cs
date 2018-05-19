using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.SeedExtensions.UnitTest
{
    public class CreateSeedExtensionsTest
    {
        [Fact]
        [Obsolete]
        public void CreateSeededAnonymousOnContainerReturnsCorrectResult()
        {
            // Arrange
            var seed = TimeSpan.FromMinutes(8);
            object expectedResult = TimeSpan.FromHours(2);
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(TimeSpan), seed)) ? expectedResult : new NoSpecimen() };
            // Act
            var result = container.CreateAnonymous(seed);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderWithSeedThrows()
        {
            // Arrange
            var dummySeed = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>(
                    (ISpecimenBuilder)null,
                    dummySeed));
        }

        [Fact]
        public void CreateSeededManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Arrange
            var seed = TimeSpan.FromMinutes(48);
            var expectedResult = Enumerable.Range(1, 8).Select(i => TimeSpan.FromHours(i));
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new MultipleRequest(new SeededRequest(typeof(TimeSpan), seed)), r);
                return expectedResult.Cast<object>();
            };

            //var composer = new DelegatingComposer { OnCreate = specimenBuilder.OnCreate };
            // Act
            var result = specimenBuilder.CreateMany(seed);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateSeededManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Arrange
            var seed = TimeSpan.FromMinutes(21);
            var expected =
                Enumerable.Range(42, 7).Select(i => TimeSpan.FromHours(i));
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(
                    new MultipleRequest(
                        new SeededRequest(typeof(TimeSpan), seed)),
                    r);
                return expected.Cast<object>();
            };
            // Act
            IEnumerable<TimeSpan> actual = builder.CreateMany(seed);
            // Assert
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
        }

        [Fact]
        public void CreateManyOnNullSpecimenContextWithSeedThrows()
        {
            // Arrange
            var dummySeed = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>((ISpecimenContext)null, dummySeed));
        }

        [Fact]
        public void CreateSeededManyOnContainerReturnsCorrectResult()
        {
            // Arrange
            var seed = new Version(1, 1);
            var expectedResult = Enumerable.Range(1, 5).Select(i => new Version(i, i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new MultipleRequest(new SeededRequest(typeof(Version), seed))) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
            };
            // Act
            var result = container.CreateMany(seed);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderWithSeedAndCountThrows()
        {
            // Arrange
            var dummySeed = new object();
            var dummyCount = 8;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>(
                    (ISpecimenBuilder)null,
                    dummySeed,
                    dummyCount));
        }

        [Fact]
        public void CreateSeededAndCountedManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Arrange
            var seed = TimeSpan.FromDays(3);
            var count = 6;
            var expectedResult = Enumerable.Range(1, count).Select(i => TimeSpan.FromHours(i));
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new FiniteSequenceRequest(new SeededRequest(typeof(TimeSpan), seed), count), r);
                return expectedResult.Cast<object>();
            };

            // Act
            var result = specimenBuilder.CreateMany(seed, count);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateSeededAndCountedManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Arrange
            var seed = TimeSpan.FromDays(4);
            var count = 5;
            var expected =
                Enumerable.Range(1, count).Select(i => TimeSpan.FromHours(i));
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(
                    new FiniteSequenceRequest(
                        new SeededRequest(typeof(TimeSpan), seed), count),
                    r);
                return expected.Cast<object>();
            };
            // Act
            IEnumerable<TimeSpan> actual = builder.CreateMany(seed, count);
            // Assert
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
        }

        [Fact]
        public void CreateManyOnNullSpecimenContextWithSeedAndCountThrows()
        {
            // Arrange
            var dummySeed = new object();
            var dummyCount = 1;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>((ISpecimenContext)null, dummySeed, dummyCount));
        }

        [Fact]
        public void CreateSeededAndCountedManyOnContainerReturnsCorrectResult()
        {
            // Arrange
            var seed = new Version(0, 9);
            var count = 4;
            var expectedResult = Enumerable.Range(1, count).Select(i => new Version(i, i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new FiniteSequenceRequest(new SeededRequest(typeof(Version), seed), count)) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
            };
            // Act
            var result = container.CreateMany(seed, count);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateFromNullSpecimenBuilderWithSeedThrows()
        {
            // Arrange
            var dummySeed = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.Create<object>((ISpecimenBuilder)null, dummySeed));
        }

        [Fact]
        public void CreateaSeededAnonymousOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Arrange
            var seed = new Version(1, 1);
            var expectedResult = new Version(2, 0);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(Version), seed), r);
                return expectedResult;
            };

            // Act
            var result = specimenBuilder.Create(seed);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithSeedOnSpecimenBuilderReturnsCorrectResult()
        {
            // Arrange
            var seed = new Version(2, 15);
            var expected = new Version(3, 0);
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(
                    new SeededRequest(typeof(Version), seed),
                    r);
                return expected;
            };
            // Act
            Version actual = builder.Create<Version>(seed);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateFromNullSpecimenContextWithSeedThrows()
        {
            // Arrange
            var dummySeed = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.Create<object>((ISpecimenContext)null, dummySeed));
        }

        [Fact]
        public void CreateSeededOnContainerReturnsCorrectResult()
        {
            // Arrange
            var seed = TimeSpan.FromMinutes(8);
            object expectedResult = TimeSpan.FromHours(2);
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(TimeSpan), seed)) ? expectedResult : new NoSpecimen() };
            // Act
            var result = container.Create(seed);
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
