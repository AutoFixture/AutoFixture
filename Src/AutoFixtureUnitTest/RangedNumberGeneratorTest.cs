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
        [InlineData(typeof(int), 10, 20, 10, 10)]
        [InlineData(typeof(int), 10, 20, 20, 20)]
        [InlineData(typeof(int), 10, 20, 21, 10)]
        [InlineData(typeof(int), 10, 20, 1, 11)]
        [InlineData(typeof(int), 10, 20, 2, 12)]
        [InlineData(typeof(int), 10, 20, 3, 13)]
        [InlineData(typeof(double), 10.1, 20.0, 1.0, 11.1)]
        [InlineData(typeof(double), 10.2, 20.0, 2.0, 12.2)]
        [InlineData(typeof(double), 10.3, 20.0, 3.0, 13.3)]
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
    }
}
