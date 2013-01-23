using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            var request = new RangedNumberRequest(operandType, minimum, maximum);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(request.OperandType, r);
                    return contextValue;
                }
            };
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(10, 2)]
        [InlineData(10, 10)]
        public void CreateReturnsCorrectResultOnMultipleCall(object maximum, int loopCount)
        {
            // Fixture setup
            var numbers = new Random();
            var request = new RangedNumberRequest(typeof(int), 1, maximum);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(request.OperandType, r);
                    return numbers.Next();
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, int>(sut => (int)sut.Create(request, context));
            // Exercise system and verify outcome
            loopTest.Execute(loopCount);
            // Teardown
        }

        [Theory]
        [InlineData(1, 10, 11, 1)]
        [InlineData(1, 10, 12, 2)]
        [InlineData(1, 10, 13, 3)]
        [InlineData(10, 20, 11, 20)]
        [InlineData(10, 20, 12, 10)]
        [InlineData(10, 20, 13, 11)]
        public void CreateReturnsCorrectResultWhenRunOutOfNumbers(int minimum, int maximum, int loopCount, int expectedResult)
        {
            // Fixture setup
            var numbers = new Random();
            var request = new RangedNumberRequest(typeof(int), minimum, maximum);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(request.OperandType, r);
                    return numbers.Next();
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, int>(sut => (int)sut.Create(request, context));
            // Exercise system and verify outcome
            loopTest.Execute(loopCount, expectedResult);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestContainsNonNumericValues()
        {
            // Fixture setup
            var request = new RangedNumberRequest(typeof(string), "1/1/2001", "1/1/2011");
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(request.OperandType, r);
                    return "14/12/1984";
                }
            };
            var loopTest = new LoopTest<RangedNumberGenerator, object>(sut => (object)sut.Create(request, context));
            // Exercise system and verify outcome
            loopTest.Execute(2, new NoSpecimen(request));
            // Teardown
        }

        [Theory]
        [InlineData("1.1", "3.3")]
        [InlineData(100.1, 300.3)]
        [InlineData(10001, 30003)]
        [InlineData(-30003, -10001)]
        [InlineData(-300.3, -100.1)]
        [InlineData("-3.3", "-1.1")]
        [UseCulture("en-US")]
        public void CreateWithDifferentOperandTypeDoesNotThrowOnMultipleCall(object minimum, object maximum)
        {
            // Fixture setup
            var numbers = new Random();
            var request = new[]
            {
                new RangedNumberRequest(
                    typeof(decimal),
                    Convert.ChangeType(minimum, typeof(decimal)),
                    Convert.ChangeType(maximum, typeof(decimal))
                    ),
                new RangedNumberRequest(
                    typeof(double),
                    Convert.ChangeType(minimum, typeof(double)),
                    Convert.ChangeType(maximum, typeof(double))
                    ),
                new RangedNumberRequest(
                    typeof(decimal),
                    Convert.ChangeType(minimum, typeof(decimal)),
                    Convert.ChangeType(maximum, typeof(decimal))
                    )
            };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (r.Equals(typeof(double)))
                        return numbers.NextDouble();
                    if (r.Equals(typeof(decimal)))
                        return Convert.ToDecimal(numbers.Next());

                    return new NoSpecimen(r);
                }
            };
            var sut = new RangedNumberGenerator();
            // Exercise system and verify outcome
            Array.ForEach(request, r => Assert.DoesNotThrow(() => sut.Create(r, context)));
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

                yield return CreateTestCase(operandType: typeof(char), minimum: 'a', maximum: 'b', contextValue: 'a', expectedResult: 'a');
                yield return CreateTestCase(operandType: typeof(char), minimum: 'a', maximum: 'b', contextValue: 'b', expectedResult: 'b');
                yield return CreateTestCase(operandType: typeof(char), minimum: 'a', maximum: 'b', contextValue: 'c', expectedResult: 'a');
                yield return CreateTestCase(operandType: typeof(char), minimum: 'b', maximum: 'c', contextValue: 'a', 
                    expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(char), 'b', 'c')));

                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue:  1, expectedResult: (byte)11);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue:  2, expectedResult: (byte)12);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue:  3, expectedResult: (byte)13);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (byte)10);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (byte)20);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (byte)10);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(byte), 10, 20)));
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
