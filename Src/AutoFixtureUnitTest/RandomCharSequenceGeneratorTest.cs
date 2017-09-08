using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomCharSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomCharSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Null(
                Record.Exception(() => new RandomCharSequenceGenerator()));
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(decimal))]
        public void CreateWithNonCharTypeRequestReturnsNoSpecimen(Type request)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system
            var result = sut.Create(typeof(char), dummyContext);
            // Verify outcome
            Assert.IsAssignableFrom<char>(result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultOnMultipleCall()
        {
            // Fixture setup
            int printableCharactersCount = 94;
            char c = '!';
            var expected = Enumerable
                .Range(1, printableCharactersCount)
                .Select(x => c++)
                .Cast<char>()
                .OrderBy(x => x);
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system
            var result = Enumerable
                .Range(1, printableCharactersCount)
                .Select(x => sut.Create(typeof(char), dummyContext))
                .Cast<char>()
                .OrderBy(x => x);
            // Verify outcome
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(94, 94)]
        [InlineData(94, 100)]
        [InlineData(94, 1000)]
        public void CreateReturnsCorrectResultOnMultipleCallWhenRunOutOfChars(
            int expected, int repeatCount)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Exercise system
            var result = Enumerable
                .Range(1, repeatCount)
                .Select(x => sut.Create(typeof(char), dummyContext))
                .Cast<char>()
                .Distinct()
                .Count();
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
