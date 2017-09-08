using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CharSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new CharSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new CharSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var sut = new CharSequenceGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
            // Teardown
        }

        [Fact]
        public void CreateWithNonCharRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new CharSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResult()
        {
            // Fixture setup
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(charRequest, dummyContext);
            // Verify outcome
            Assert.Equal('!', result);
            // Teardown
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResultOnSecondCall()
        {
            // Fixture setup
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Range(1, 2)
                .Select(i => sut.Create(charRequest, dummyContext))
                .Cast<char>();
            // Verify outcome
            char c = ' ';
            IEnumerable<char> expectedResult = Enumerable.Range(1, 2).Select(i => ++c);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResultOnTenthCall()
        {
            // Fixture setup
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Range(1, 10)
                .Select(i => sut.Create(charRequest, dummyContext))
                .Cast<char>();
            // Verify outcome
            char c = '!';
            IEnumerable<char> expectedResult = Enumerable.Range(1, 10).Select(i => c++);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateWithCharReturnsCorrectResultWhenRunOutOfChars()
        {
            // Fixture setup
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Range(1, 95)
                .Select(i => sut.Create(charRequest, dummyContext))
                .Cast<char>()
                .Last();
            // Verify outcome
            char expectedResult = '!';
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
