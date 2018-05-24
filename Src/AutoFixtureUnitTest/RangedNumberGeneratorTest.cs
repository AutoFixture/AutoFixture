using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RangedNumberGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RangedNumberGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new RangedNumberGenerator();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RangedNumberGenerator();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Theory]
        [ClassData(typeof(RangedNumberRequestTestCases))]
        public void CreateReturnsCorrectResult(Type operandType, object minimum, object maximum, object contextValue, object expectedResult)
        {
            // Arrange
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
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(10, 2)]
        [InlineData(10, 10)]
        public void CreateReturnsCorrectResultOnMultipleCall(object maximum, int loopCount)
        {
            // Arrange
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
            // Act & assert
            loopTest.Execute(loopCount);
        }

        [Theory]
        [InlineData(10.1, 20.2)]
        [InlineData(10.0, 20.2)]
        [InlineData(10.1, 20.0)]
        [InlineData(-10.1, 10.2)]
        [InlineData(-10.0, 10.2)]
        [InlineData(-10.1, 10.0)]
        public void CreateReturnsCorrectResultWithMinimumMaximumOnMultipleCall(
            double minimum,
            double maximum)
        {
            // Arrange
            var numbers = new Random();
            var request = new RangedNumberRequest(
                typeof(double),
                minimum,
                maximum);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(request.OperandType, r);
                    return (double)numbers.Next();
                }
            };
            var sut = new RangedNumberGenerator();
            // Act
            var result = Enumerable
                .Range(0, 33)
                .Select(x => sut.Create(request, context))
                .Cast<double>();
            // Assert
            Assert.True(
                result.All(x => x >= minimum && x <= maximum));
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
            // Arrange
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
            // Act & assert
            loopTest.Execute(loopCount, expectedResult);
        }

        [Theory]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        public void ReturnsSameNumberWhenMinimumAndMaximumAreSame(Type requestType)
        {
            // Arrange
            var range = Convert.ChangeType(42, requestType, CultureInfo.InvariantCulture);
            var sut = new RangedNumberGenerator();
            var request = new RangedNumberRequest(requestType, range, range);

            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.Equal(range, result);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestContainsNonNumericValues()
        {
            // Arrange
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
            // Act & assert
            loopTest.Execute(2, new NoSpecimen());
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
            // Arrange
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

                    return new NoSpecimen();
                }
            };
            var sut = new RangedNumberGenerator();
            // Act & assert
            foreach (var r in request)
            {
                Assert.Null(Record.Exception(() => sut.Create(r, context)));
            }
        }

        [Fact]
        public void DoesNotThrowForHeterogeneousRequestsWithValuesGreaterThanByteMaxValue()
        {
            var testCases = new[]
            {
                new
                {
                    request =
                        new RangedNumberRequest(
                            typeof(byte), byte.MinValue, byte.MaxValue),
                    contextStub =
                        new DelegatingSpecimenContext
                        {
                            OnResolve = r => byte.MaxValue
                        }
                },
                new
                {
                    request =
                        new RangedNumberRequest(
                            typeof(short), (short)(byte.MaxValue), short.MaxValue),
                    contextStub =
                        new DelegatingSpecimenContext
                        {
                            OnResolve = r => short.MaxValue
                        }
                },
                new
                {
                    request =
                        new RangedNumberRequest(
                            typeof(byte), byte.MinValue, byte.MaxValue),
                    contextStub =
                        new DelegatingSpecimenContext
                        {
                            OnResolve = r => int.MaxValue
                        }
                }
            };
            var sut = new RangedNumberGenerator();

            Func<int, object> actual = i =>
                sut.Create(testCases[i].request, testCases[i].contextStub);

            Assert.Null(Record.Exception(() =>
                Enumerable.Range(0, testCases.Length).Select(actual).ToList()));
        }

        [Fact]
        public void DoesNotThrowForHeterogeneousRequestsWithValuesGreaterThanInt16MaxValue()
        {
            var testCases = new[]
            {
                new
                {
                    request =
                        new RangedNumberRequest(
                            typeof(short), (short)byte.MaxValue, short.MaxValue),
                    contextStub =
                        new DelegatingSpecimenContext
                        {
                            OnResolve = r => short.MaxValue
                        }
                },
                new
                {
                    request =
                        new RangedNumberRequest(
                            typeof(int), (int)(short.MaxValue), int.MaxValue),
                    contextStub =
                        new DelegatingSpecimenContext
                        {
                            OnResolve = r => int.MaxValue
                        }
                },
                new
                {
                    request =
                        new RangedNumberRequest(
                            typeof(short), (short)byte.MaxValue, short.MaxValue),
                    contextStub =
                        new DelegatingSpecimenContext
                        {
                            OnResolve = r => short.MaxValue
                        }
                }
            };
            var sut = new RangedNumberGenerator();

            Func<int, object> actual = i =>
                sut.Create(testCases[i].request, testCases[i].contextStub);

            Assert.Null(Record.Exception(() =>
                Enumerable.Range(0, testCases.Length).Select(actual).ToList()));
        }

        private sealed class RangedNumberRequestTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: 1, expectedResult: -5);
                yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: -1, expectedResult: -1);
                yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: 0, expectedResult: -5);
                yield return CreateTestCase(operandType: typeof(int), minimum: 1, maximum: 3, contextValue: -9, expectedResult: 1);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 1, expectedResult: 11);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 2, expectedResult: 12);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 3, expectedResult: 13);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 10, expectedResult: 10);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 17, expectedResult: 17);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 20, expectedResult: 20);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 21, expectedResult: 10);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 13, contextValue: 4, expectedResult: 10);
                yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 1, expectedResult: (uint)11);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 2, expectedResult: (uint)12);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 3, expectedResult: (uint)13);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (uint)10);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 17, expectedResult: (uint)17);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (uint)20);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (uint)10);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 13, contextValue: 4, expectedResult: (uint)10);
                yield return CreateTestCase(operandType: typeof(uint), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue: 1.0, expectedResult: -5.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue: -1.0, expectedResult: -1.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue: 0.0, expectedResult: -5.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 1.0, maximum: 3.0, contextValue: -9.0, expectedResult: 1.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 1.0, expectedResult: 11.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 2.0, expectedResult: 12.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 3.0, expectedResult: 13.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 10.0, expectedResult: 10.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 17.0, expectedResult: 17.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 20.0, expectedResult: 20.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 21.0, expectedResult: 10.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 13.0, contextValue: 4.0, expectedResult: 10.0);
                yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue: 10000000000, expectedResult: -50000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue: -10000000000, expectedResult: -10000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue: 10000000000, expectedResult: -50000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 300000000000, contextValue: -90000000000, expectedResult: 100000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 10000000000, expectedResult: 110000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 20000000000, expectedResult: 120000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 30000000000, expectedResult: 130000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 100000000000, expectedResult: 100000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 170000000000, expectedResult: 170000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 200000000000, expectedResult: 200000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 210000000000, expectedResult: 100000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 130000000000, contextValue: 40000000000, expectedResult: 100000000000);
                yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 1, expectedResult: (ulong)11);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 2, expectedResult: (ulong)12);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 3, expectedResult: (ulong)13);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (ulong)10);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 17, expectedResult: (ulong)17);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (ulong)20);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (ulong)10);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 13, contextValue: 4, expectedResult: (ulong)10);
                yield return CreateTestCase(operandType: typeof(ulong), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue: 1.0m, expectedResult: -5.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue: -1.0m, expectedResult: -1.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue: 0.0m, expectedResult: -5.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 1.0m, maximum: 3.0m, contextValue: -9.0m, expectedResult: 1.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 1.0m, expectedResult: 11.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 2.0m, expectedResult: 12.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 3.0m, expectedResult: 13.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 10.0m, expectedResult: 10.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 17.0m, expectedResult: 17.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 20.0m, expectedResult: 20.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 21.0m, expectedResult: 10.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 13.0m, contextValue: 4.0m, expectedResult: 10.0m);
                yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(char), minimum: 'a', maximum: 'b', contextValue: 'a', expectedResult: 'a');
                yield return CreateTestCase(operandType: typeof(char), minimum: 'a', maximum: 'b', contextValue: 'b', expectedResult: 'b');
                yield return CreateTestCase(operandType: typeof(char), minimum: 'a', maximum: 'b', contextValue: 'c', expectedResult: 'a');
                yield return CreateTestCase(operandType: typeof(char), minimum: 'b', maximum: 'c', contextValue: 'a',
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 1, expectedResult: (byte)11);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 2, expectedResult: (byte)12);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 3, expectedResult: (byte)13);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (byte)10);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 17, expectedResult: (byte)17);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (byte)20);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (byte)10);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 13, contextValue: 4, expectedResult: (byte)10);
                yield return CreateTestCase(operandType: typeof(byte), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(sbyte), minimum: -5, maximum: -1, contextValue: 1, expectedResult: (sbyte)-5);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: -5, maximum: -1, contextValue: -1, expectedResult: (sbyte)-1);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: -5, maximum: -1, contextValue: 0, expectedResult: (sbyte)-5);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 1, maximum: 3, contextValue: -9, expectedResult: (sbyte)1);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 1, expectedResult: (sbyte)11);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 2, expectedResult: (sbyte)12);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 3, expectedResult: (sbyte)13);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (sbyte)10);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 17, expectedResult: (sbyte)17);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (sbyte)20);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (sbyte)10);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 13, contextValue: 4, expectedResult: (sbyte)10);
                yield return CreateTestCase(operandType: typeof(sbyte), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(float), minimum: -5.0f, maximum: -1.0f, contextValue: 1.0f, expectedResult: -5.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: -5.0f, maximum: -1.0f, contextValue: -1.0f, expectedResult: -1.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: -5.0f, maximum: -1.0f, contextValue: 0.0f, expectedResult: -5.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 1.0f, maximum: 3.0f, contextValue: -9.0f, expectedResult: 1.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 1.0f, expectedResult: 11.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 2.0f, expectedResult: 12.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 3.0f, expectedResult: 13.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 10.0f, expectedResult: 10.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 17.0f, expectedResult: 17.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 20.0f, expectedResult: 20.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: 21.0f, expectedResult: 10.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 13.0f, contextValue: 4.0f, expectedResult: 10.0f);
                yield return CreateTestCase(operandType: typeof(float), minimum: 10.0f, maximum: 20.0f, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(short), minimum: -5, maximum: -1, contextValue: 1, expectedResult: (short)-5);
                yield return CreateTestCase(operandType: typeof(short), minimum: -5, maximum: -1, contextValue: -1, expectedResult: (short)-1);
                yield return CreateTestCase(operandType: typeof(short), minimum: -5, maximum: -1, contextValue: 0, expectedResult: (short)-5);
                yield return CreateTestCase(operandType: typeof(short), minimum: 1, maximum: 3, contextValue: -9, expectedResult: (short)1);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 1, expectedResult: (short)11);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 2, expectedResult: (short)12);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 3, expectedResult: (short)13);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (short)10);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 17, expectedResult: (short)17);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (short)20);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (short)10);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 13, contextValue: 4, expectedResult: (short)10);
                yield return CreateTestCase(operandType: typeof(short), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());

                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 1, expectedResult: (ushort)11);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 2, expectedResult: (ushort)12);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 3, expectedResult: (ushort)13);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 10, expectedResult: (ushort)10);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 17, expectedResult: (ushort)17);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 20, expectedResult: (ushort)20);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: 21, expectedResult: (ushort)10);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 13, contextValue: 4, expectedResult: (ushort)10);
                yield return CreateTestCase(operandType: typeof(ushort), minimum: 10, maximum: 20, contextValue: new object(),
                    expectedResult: new NoSpecimen());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private static object[] CreateTestCase(Type operandType, object minimum, object maximum, object contextValue, object expectedResult)
            {
                return new object[] { operandType, minimum, maximum, contextValue, expectedResult };
            }
        }
    }
}
