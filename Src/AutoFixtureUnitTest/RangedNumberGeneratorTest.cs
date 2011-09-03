using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RangedNumberGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RangedNumberGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new RangedNumberGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RangedNumberGenerator();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(dummyRequest), result);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(RangedNumberGeneratorDataTest))]
        public void CreateReturnsCorrectResult(Type operandType, object minimum, object maximum, object contextValue, object expectedResult)
        {
            // Fixture setup
            var sut = new RangedNumberGenerator();
            var dummyRequest = new RangedNumberRequest(operandType, minimum, maximum);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(dummyRequest, r);
                    return contextValue;
                }
            };
            // Exercise system
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultOnSecondCall()
        {
            // Fixture setup
            var dummyNumbers = new Random();
            var dummyRequest = new RangedNumberRequest(typeof(int), 1, 10);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(dummyRequest, r);
                    return dummyNumbers.Next();
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, int>(sut => (int)sut.Create(dummyRequest, dummyContext));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultOnTenthCall()
        {
            // Fixture setup
            var dummyNumbers = new Random();
            var dummyRequest = new RangedNumberRequest(typeof(int), 1, 10);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(dummyRequest, r);
                    return dummyNumbers.Next();
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, int>(sut => (int)sut.Create(dummyRequest, dummyContext));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }

        [Theory]
        [InlineData(11, 1)]
        [InlineData(12, 2)]
        [InlineData(13, 3)]
        public void CreateReturnsCorrectResultWhenRunOutOfNumbers(int loopCount, int expectedResult)
        {
            // Fixture setup
            var dummyNumbers = new Random();
            var dummyRequest = new RangedNumberRequest(typeof(int), 1, 10);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(dummyRequest, r);
                    return dummyNumbers.Next();
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, int>(sut => (int)sut.Create(dummyRequest, dummyContext));
            // Exercise system and verify outcome
            loopTest.Execute(loopCount, expectedResult);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestContainsNonNumericValues()
        {
            // Fixture setup
            var dummyRequest = new RangedNumberRequest(typeof(string), "1/1/2001", "1/1/2011");
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(dummyRequest, r);
                    return "14/12/1984";
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, object>(sut => (object)sut.Create(dummyRequest, dummyContext));
            // Exercise system and verify outcome
            loopTest.Execute(2, new NoSpecimen(dummyRequest));
            // Teardown
        }
    }
}
