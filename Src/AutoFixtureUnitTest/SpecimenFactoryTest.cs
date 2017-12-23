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
        [Fact]
        [Obsolete]
        public void CreateAnonymousFromNullSpecimenContextThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateAnonymous<object>((ISpecimenContext)null));
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
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.Create<object>((ISpecimenContext)null));
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousOnContainerReturnsCorrectResult()
        {
            // Arrange
            object expectedResult = 1;
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(int), 0)) ? expectedResult : new NoSpecimen() };
            // Act
            var result = container.CreateAnonymous<int>();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateOnContainerReturnsCorrectResult()
        {
            // Arrange
            object expectedResult = 1;
            var container = new DelegatingSpecimenContext { OnResolve = r => r.Equals(new SeededRequest(typeof(int), 0)) ? expectedResult : new NoSpecimen() };
            // Act
            var result = container.Create<int>();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateAnonymousOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = new DateTime(2010, 5, 31, 14, 52, 19);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(DateTime), default(DateTime)), r);
                return expectedResult;
            };

            ISpecimenBuilder composer = new DelegatingComposer { OnCreate = specimenBuilder.OnCreate };
            // Act
            var result = composer.Create<DateTime>();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateOnSpecimenBuilderReturnsCorrectResult()
        {
            // Arrange
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
            // Act
            DateTime actual = builder.Create<DateTime>();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousOnPostprocessComposerReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = new DateTime(2010, 5, 31, 14, 52, 19);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(DateTime), default(DateTime)), r);
                return expectedResult;
            };

            var composer = new DelegatingComposer<DateTime> { OnCreate = specimenBuilder.OnCreate };
            // Act
            var result = composer.CreateAnonymous();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateOnPostprocessComposerReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = new DateTime(2010, 5, 31, 14, 52, 19);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new SeededRequest(typeof(DateTime), default(DateTime)), r);
                return expectedResult;
            };

            var composer = new DelegatingComposer<DateTime> { OnCreate = specimenBuilder.OnCreate };
            // Act
            var result = composer.Create();
            // Assert
            Assert.Equal(expectedResult, result);
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
            // Arrange
            var dummyCount = 10;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenFactory.CreateMany<string>(
                    (ISpecimenBuilder)null,
                    dummyCount));
        }

        [Fact]
        public void CreateManyOnContainerReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = Enumerable.Range(1, 10);
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new MultipleRequest(new SeededRequest(typeof(int), 0))) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
            };
            // Act
            var result = container.CreateMany<int>();
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = Enumerable.Range(1, 17).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    Assert.Equal(new MultipleRequest(new SeededRequest(typeof(string), null)), r);
                    return expectedResult.Cast<object>();
                };

            ISpecimenBuilder composer = new DelegatingComposer { OnCreate = specimenBuilder.OnCreate };
            // Act
            var result = composer.CreateMany<string>();
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Arrange
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
            // Act
            IEnumerable<string> actual = builder.CreateMany<string>();
            // Assert
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
        }

        [Fact]
        public void CreateManyOnPostprocessComposerReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = Enumerable.Range(1, 17).Select(i => i.ToString());
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(new MultipleRequest(new SeededRequest(typeof(string), null)), r);
                return expectedResult.Cast<object>();
            };

            var composer = new DelegatingComposer<string> { OnCreate = specimenBuilder.OnCreate };
            // Act
            var result = composer.CreateMany();
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateCountedManyOnContainerReturnsCorrectResult()
        {
            // Arrange
            var count = 19;
            var expectedResult = Enumerable.Range(1, count).Select(i => new DateTime(i));
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => r.Equals(new FiniteSequenceRequest(new SeededRequest(typeof(DateTime), default(DateTime)), count)) ?
                    (object)expectedResult.Cast<object>() :
                    new NoSpecimen()
            };
            // Act
            var result = container.CreateMany<DateTime>(count);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateCountedManyOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Arrange
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
            // Act
            var result = composer.CreateMany<string>(count);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateCountedManyOnSpecimenBuilderReturnsCorrectResult()
        {
            // Arrange
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
            // Act
            IEnumerable<string> actual = builder.CreateMany<string>(count);
            // Assert
            Assert.True(
                expected.SequenceEqual(actual),
                "Sequences not equal.");
        }

        [Fact]
        public void CreateCountedManyOnPostprocessComposerReturnsCorrectResult()
        {
            // Arrange
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
            // Act
            var result = composer.CreateMany(count);
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }
    }
}
