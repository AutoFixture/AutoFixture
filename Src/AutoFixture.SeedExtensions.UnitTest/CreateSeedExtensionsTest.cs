using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.SeedExtensions.UnitTest
{
    public class CreateSeedExtensionsTest
    {
        [Fact]
        [Obsolete]
        public void CreateSeededAnonymousOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = TimeSpan.FromMinutes(8);
            object expectedResult = TimeSpan.FromHours(2);
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(TimeSpan), seed)) ? expectedResult : new NoSpecimen() };
            // Exercise system
            var result = container.CreateAnonymous(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>(
                    (ISpecimenBuilder)null,
                    dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateSeededManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
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
            // Exercise system
            var result = specimenBuilder.CreateMany(seed);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }


        [Fact]
        public void CreateSeededManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Fixture setup
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
            // Exercise system
            IEnumerable<TimeSpan> actual = builder.CreateMany(seed);
            // Verify outcome
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
            // Teardown
        }


        [Fact]
        public void CreateManyOnNullSpecimenContextWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>((ISpecimenContext)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateSeededManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = new Version(1, 1);
            var expectedResult = Enumerable.Range(1, 5).Select(i => new Version(i, i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new MultipleRequest(new SeededRequest(typeof(Version), seed))) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
            };
            // Exercise system
            var result = container.CreateMany(seed);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }


        [Fact]
        public void CreateManyOnNullSpecimenBuilderWithSeedAndCountThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            var dummyCount = 8;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>(
                    (ISpecimenBuilder)null,
                    dummySeed,
                    dummyCount));
            // Teardown
        }


        [Fact]
        public void CreateSeededAndCountedManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
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

            // Exercise system
            var result = specimenBuilder.CreateMany(seed, count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }


        [Fact]
        public void CreateSeededAndCountedManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Fixture setup
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
            // Exercise system
            IEnumerable<TimeSpan> actual = builder.CreateMany(seed, count);
            // Verify outcome
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
            // Teardown
        }


        [Fact]
        public void CreateManyOnNullSpecimenContextWithSeedAndCountThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            var dummyCount = 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.CreateMany<object>((ISpecimenContext)null, dummySeed, dummyCount));
            // Teardown
        }


        [Fact]
        public void CreateSeededAndCountedManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = new Version(0, 9);
            var count = 4;
            var expectedResult = Enumerable.Range(1, count).Select(i => new Version(i, i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new FiniteSequenceRequest(new SeededRequest(typeof(Version), seed), count)) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
            };
            // Exercise system
            var result = container.CreateMany(seed, count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }


        [Fact]
        public void CreateFromNullSpecimenBuilderWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.Create<object>((ISpecimenBuilder)null, dummySeed));
            // Teardown
        }


        [Fact]
        public void CreateaSeededAnonymousOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = new Version(1, 1);
            var expectedResult = new Version(2, 0);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(Version), seed), r);
                return expectedResult;
            };

            // Exercise system
            var result = specimenBuilder.Create(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }


        [Fact]
        public void CreateWithSeedOnSpecimenBuilderReturnsCorrectResult()
        {
            // Fixture setup
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
            // Exercise system
            Version actual = builder.Create<Version>(seed);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateFromNullSpecimenContextWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                CreateSeedExtensions.Create<object>((ISpecimenContext)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateSeededOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = TimeSpan.FromMinutes(8);
            object expectedResult = TimeSpan.FromHours(2);
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(TimeSpan), seed)) ? expectedResult : new NoSpecimen() };
            // Exercise system
            var result = container.Create(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
