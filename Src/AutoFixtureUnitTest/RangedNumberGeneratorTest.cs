using System;
using System.Collections;
using System.Collections.Generic;
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
        [ClassData(typeof(RangedNumberRequestTestCases))]
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

        private sealed class RangedNumberRequestTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue:  1, expectedResult: -5);
                yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: -1, expectedResult: -1);
                yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue:  0, expectedResult: -5);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue:  1, expectedResult: 11);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue:  2, expectedResult: 12);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue:  3, expectedResult: 13);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 10, expectedResult: 10);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 20, expectedResult: 20);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 21, expectedResult: 10);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: new object(), 
                    expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(int), 10, 20)));

                yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue:  1.0, expectedResult: -5.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue: -1.0, expectedResult: -1.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue:  0.0, expectedResult: -5.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  1.0, expectedResult: 11.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  2.0, expectedResult: 12.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  3.0, expectedResult: 13.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 10.0, expectedResult: 10.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 20.0, expectedResult: 20.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 21.0, expectedResult: 10.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: new object(), 
                    expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(double), 10.0, 20.0)));

                yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue:  10000000000, expectedResult: -50000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue: -10000000000, expectedResult: -10000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue:  10000000000, expectedResult: -50000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue:  10000000000, expectedResult: 110000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue:  20000000000, expectedResult: 120000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue:  30000000000, expectedResult: 130000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 100000000000, expectedResult: 100000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 200000000000, expectedResult: 200000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 210000000000, expectedResult: 100000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: new object(), 
                    expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(long), 100000000000, 200000000000)));

                yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue:  1.0m, expectedResult: -5.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue: -1.0m, expectedResult: -1.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue:  0.0m, expectedResult: -5.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue:  1.0m, expectedResult: 11.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue:  2.0m, expectedResult: 12.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue:  3.0m, expectedResult: 13.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 10.0m, expectedResult: 10.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 20.0m, expectedResult: 20.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 21.0m, expectedResult: 10.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: new object(), 
                    expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(decimal), 10.0m, 20.0m)));
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private static object[] CreateTestCase(Type operandType, object minimum, object maximum, object contextValue, object expectedResult)
            {
                return new object[] { operandType, minimum, maximum, contextValue, expectedResult };
            }
        }
    }
}
