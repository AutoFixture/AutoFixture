using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Dsl;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SpecimenFactoryTest
    {
        [Fact][Obsolete]
        public void CreateAnonymousFromNullSpecimenContextThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateAnonymous<object>((ISpecimenContext)null));
            // Teardown
        }

        [Fact]
        public void CreateFromNullSpecimenBuilderThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.Create<object>((ISpecimenBuilder)null));
        }

        [Fact]
        public void CreateFromNullSpecimenContextThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.Create<object>((ISpecimenContext)null));
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            object expectedResult = 1;
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(int), 0)) ? expectedResult : new NoSpecimen() };
            // Exercise system
            var result = container.CreateAnonymous<int>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            object expectedResult = 1;
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(int), 0)) ? expectedResult : new NoSpecimen() };
            // Exercise system
            var result = container.Create<int>();
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

            ISpecimenBuilder composer = new DelegatingComposer { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.Create<DateTime>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateOnSpecimenBuilderReturnsCorrectResult()
        {
            // Fixture setup
            var expected = new DateTime(2012, 11, 20, 9, 45, 51);
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(
                    new SeededRequest(typeof(DateTime), default(DateTime)),
                    r);
                return expected;
            };
            // Exercise system
            DateTime actual = builder.Create<DateTime>();
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact][Obsolete]
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

            var composer = new DelegatingComposer<DateTime> { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateOnPostprocessComposerReturnsCorrectResult()
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

            var composer = new DelegatingComposer<DateTime> { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.Create();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<object>((ISpecimenBuilder)null));
        }

        [Fact]
        public void CreateManyOnNullSpecimenBuilderWithCountThrows()
        {
            // Fixture setup
            var dummyCount = 10;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<string>(
                    (ISpecimenBuilder)null,
                    dummyCount));
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
                    new NoSpecimen()
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

            ISpecimenBuilder composer = new DelegatingComposer { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.CreateMany<string>();
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Fixture setup
            var expected =
                Enumerable.Range(1337, 42).Select(i => i.ToString());
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(
                    new MultipleRequest(
                        new SeededRequest(typeof(string), null)),
                    r);
                return expected.Cast<object>();
            };
            // Exercise system
            IEnumerable<string> actual = builder.CreateMany<string>();
            // Verify outcome
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
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

            var composer = new DelegatingComposer<string> { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.CreateMany();
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateCountedManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var count = 19;
            var expectedResult = Enumerable.Range(1, count).Select(i => new DateTime(i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new FiniteSequenceRequest(new SeededRequest(typeof(DateTime), default(DateTime)), count)) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
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
            var count = 9;
            var expectedResult = Enumerable.Range(1, count).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    Assert.Equal(new FiniteSequenceRequest(new SeededRequest(typeof(string), null), count), r);
                    return expectedResult.Cast<object>();
                };

            ISpecimenBuilder composer = new DelegatingComposer { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.CreateMany<string>(count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateCountedManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Fixture setup
            var count = 31;
            var expected = 
                Enumerable.Range(1, count).Select(i => i.ToString());
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(
                    new FiniteSequenceRequest(
                        new SeededRequest(typeof(string), null), count),
                    r);
                return expected.Cast<object>();
            };
            // Exercise system
            IEnumerable<string> actual = builder.CreateMany<string>(count);
            // Verify outcome
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
            // Teardown
        }

        [Fact]
        public void CreateCountedManyOnPostprocessComposerReturnsCorrectResult()
        {
            // Fixture setup
            var count = 9;
            var expectedResult = Enumerable.Range(1, count).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new FiniteSequenceRequest(new SeededRequest(typeof(string), null), count), r);
                return expectedResult.Cast<object>();
            };

            var composer = new DelegatingComposer<string> { OnCreate = specimenBuilder.OnCreate };
            // Exercise system
            var result = composer.CreateMany(count);
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }
    }
}
