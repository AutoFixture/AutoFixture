﻿using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Dsl;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenFactoryTest
    {
        [Fact]
        public void CreateAnonymousFromNullSpecimenBuilderComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateAnonymous<object>((ISpecimenBuilderComposer)null));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousFromNullSpecimenContextThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateAnonymous<object>((ISpecimenContext)null));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousFromNullSpecimenBuilderComposerWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateAnonymous<object>((ISpecimenBuilderComposer)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousFromNullSpecimenContextWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateAnonymous<object>((ISpecimenContext)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            object expectedResult = 1;
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(int), 0)) ? expectedResult : new NoSpecimen(r) };
            // Exercise system
            var result = container.CreateAnonymous<int>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = new DateTime(2010, 5, 31, 14, 52, 19);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(DateTime), default(DateTime)), r);
                return expectedResult;
            };

            ISpecimenBuilderComposer composer = new DelegatingComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateAnonymous<DateTime>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnPostprocessComposerReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = new DateTime(2010, 5, 31, 14, 52, 19);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(DateTime), default(DateTime)), r);
                return expectedResult;
            };

            var composer = new DelegatingComposer<DateTime> { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateSeededAnonymousOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = TimeSpan.FromMinutes(8);
            object expectedResult = TimeSpan.FromHours(2);
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(TimeSpan), seed)) ? expectedResult : new NoSpecimen(r) };
            // Exercise system
            var result = container.CreateAnonymous(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
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

            var composer = new DelegatingComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateAnonymous(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenBuilderComposer)null));
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderComposerWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenBuilderComposer)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenContextWithSeedThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenContext)null, dummySeed));
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderComposerWithCountThrows()
        {
            // Fixture setup
            const int dummyCount = 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenBuilderComposer)null, dummyCount));
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderComposerWithSeedAndCountThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            const int dummyCount = 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenBuilderComposer)null, dummySeed, dummyCount));
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenContextWithSeedAndCountThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            const int dummyCount = 1;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenContext)null, dummySeed, dummyCount));
            // Teardown
        }

        [Fact]
        public void CreateManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = Enumerable.Range(1, 10);
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new MultipleRequest(new SeededRequest(typeof(int), 0))) ? 
                    (object)expectedResult.Cast<object>() : 
                    new NoSpecimen(r) 
            };
            // Exercise system
            var result = container.CreateMany<int>();
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = Enumerable.Range(1, 17).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    Assert.Equal(new MultipleRequest(new SeededRequest(typeof(string), null)), r);
                    return expectedResult.Cast<object>();
                };

            ISpecimenBuilderComposer composer = new DelegatingComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateMany<string>();
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateManyOnPostprocessComposerReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = Enumerable.Range(1, 17).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new MultipleRequest(new SeededRequest(typeof(string), null)), r);
                return expectedResult.Cast<object>();
            };

            var composer = new DelegatingComposer<string> { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateMany();
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
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
                    new NoSpecimen(r)
            };
            // Exercise system
            var result = container.CreateMany(seed);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
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

            var composer = new DelegatingComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateMany(seed);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateCountedManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            const int count = 19;
            var expectedResult = Enumerable.Range(1, count).Select(i => new DateTime(i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new FiniteSequenceRequest(new SeededRequest(typeof(DateTime), default(DateTime)), count)) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen(r)
            };
            // Exercise system
            var result = container.CreateMany<DateTime>(count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateCountedManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            const int count = 9;
            var expectedResult = Enumerable.Range(1, count).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    Assert.Equal(new FiniteSequenceRequest(new SeededRequest(typeof(string), null), count), r);
                    return expectedResult.Cast<object>();
                };

            ISpecimenBuilderComposer composer = new DelegatingComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateMany<string>(count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateCountedManyOnPostprocessComposerReturnsCorrectResult()
        {
            // Fixture setup
            const int count = 9;
            var expectedResult = Enumerable.Range(1, count).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new FiniteSequenceRequest(new SeededRequest(typeof(string), null), count), r);
                return expectedResult.Cast<object>();
            };

            var composer = new DelegatingComposer<string> { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateMany(count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateSeededAndCountedManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = new Version(0, 9);
            const int count = 4;
            var expectedResult = Enumerable.Range(1, count).Select(i => new Version(i, i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new FiniteSequenceRequest(new SeededRequest(typeof(Version), seed), count)) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen(r)
            };
            // Exercise system
            var result = container.CreateMany(seed, count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateSeededAndCountedManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = TimeSpan.FromDays(3);
            const int count = 6;
            var expectedResult = Enumerable.Range(1, count).Select(i => TimeSpan.FromHours(i));
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    Assert.Equal(new FiniteSequenceRequest(new SeededRequest(typeof(TimeSpan), seed), count), r);
                    return expectedResult.Cast<object>();
                };

            var composer = new DelegatingComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateMany(seed, count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }
    }
}
