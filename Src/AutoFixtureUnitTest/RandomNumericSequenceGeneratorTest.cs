using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomNumericSequenceGeneratorTest
    {
        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Arrange
            // Act & assert
            Assert.Null(Record.Exception(() => new RandomNumericSequenceGenerator()));
        }

        [Fact]
        public void InitializeWithDefaultConstructorSetsCorrectLimits()
        {
            // Arrange
            var sut = new RandomNumericSequenceGenerator();
            var expectedResult = new long[]
            {
                1,
                Byte.MaxValue,
                Int16.MaxValue,
                Int32.MaxValue
            };
            // Act
            IEnumerable<long> result = sut.Limits;
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Arrange
            IEnumerable<long> nullEnumerable = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullEnumerable));
        }

        [Theory]
        [InlineData(new long[] { 1 })]
        [InlineData(new long[] { 20 })]
        [InlineData(new long[] { 300 })]
        public void InitializeWithSingleLimitThrows(long[] limits)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(() =>
                new RandomNumericSequenceGenerator(limits));
        }

        [Theory]
        [InlineData(new long[] { 10, 5 })]
        [InlineData(new long[] { 32, 8, Int32.MaxValue })]
        [InlineData(new long[] { 0, -2, 5 })]
        [InlineData(new long[] { -4, -8 })]
        [InlineData(new long[] { 1, 1, 5 })]
        [InlineData(new long[] { 1, 5, 5 })]
        public void InitializeWithLimitsNotAscendingThrows(long[] limits)
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RandomNumericSequenceGenerator(limits));
        }

        [Fact]
        public void LimitsMatchListParameter()
        {
            // Arrange
            var expectedResult = new long[] { 10, 20, 30 }.AsEnumerable();
            var sut = new RandomNumericSequenceGenerator(expectedResult);
            // Act
            IEnumerable<long> result = sut.Limits;
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            long[] nullArray = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullArray));
        }

        [Fact]
        public void LimitsMatchParamsArray()
        {
            // Arrange
            var expectedResult = new long[] { 10, 20, 30 };
            var sut = new RandomNumericSequenceGenerator(expectedResult);
            // Act
            IEnumerable<long> result = sut.Limits;
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RandomNumericSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator();
            // Act
            object result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var dummyRequest = typeof(byte);
            var sut = new RandomNumericSequenceGenerator();
            // Act & assert
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new NoSpecimen();
            var sut = new RandomNumericSequenceGenerator();
            // Act
            object result = sut.Create(request, dummyContext);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(bool))]
        public void CreateWithNonNumericTypeRequestReturnsNoSpecimen(Type request)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new NoSpecimen();
            var sut = new RandomNumericSequenceGenerator();
            // Act
            object result = sut.Create(request, dummyContext);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(byte))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(double))]
        [InlineData(typeof(short))]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(float))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(ulong))]
        public void CreateWithNumericTypeRequestReturnsValueOfSameType(Type request)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator();
            // Act
            object result = sut.Create(request, dummyContext);
            // Assert
            Assert.IsType(request, result);
        }

        [Theory]
        [MemberData(nameof(LimitSequenceTestCases))]
        public void CreateReturnsNumberInCorrectRange(long[] limits)
        {
            // Arrange
            var request = typeof(double);
            var dummyContext = new DelegatingSpecimenContext();
            var expectedMin = (int)limits.Min();
            var expectedMax = (int)limits.Max();
            var sut = new RandomNumericSequenceGenerator(limits);
            // Act
            var result = (double)sut.Create(request, dummyContext);
            // Assert
            Assert.True(
                result >= expectedMin && result <= expectedMax
                );
        }

        [Theory]
        [MemberData(nameof(LimitSequenceTestCases))]
        public void CreateReturnsNumberInCorrectRangeOnMultipleCall(long[] limits)
        {
            // Arrange
            var request = typeof(int);
            var dummyContext = new DelegatingSpecimenContext();
            var expectedMin = (int)limits.Min();
            var expectedMax = (int)limits.Max();
            int repeatCount = (expectedMax - expectedMin) + 1;
            var sut = new RandomNumericSequenceGenerator(limits);
            // Act
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(request, dummyContext))
                .Cast<int>();
            // Assert
            Assert.True(
                result.All(x => x >= expectedMin && x <= expectedMax)
                );
        }

        [Theory]
        [InlineData(new[] { long.MinValue, long.MaxValue })]
        [InlineData(new[] { 900000, (long)int.MaxValue + 3 })]
        [InlineData(new[] { int.MinValue, long.MaxValue - 3 })]
        [InlineData(new[] { long.MinValue, long.MaxValue - 3 })]
        [InlineData(new[] { int.MinValue + 100, long.MaxValue })]
        [InlineData(new[] { long.MinValue + 200, long.MaxValue })]
        [InlineData(new[] { long.MinValue, int.MinValue + 90000 })]
        [InlineData(new[] { byte.MaxValue + 10000, long.MaxValue })]
        [InlineData(new[] { long.MinValue + 200000, long.MaxValue })]
        [InlineData(new[] { (long)int.MaxValue + 10, long.MaxValue })]
        [InlineData(new[] { (long)int.MaxValue + 200, long.MaxValue })]
        [InlineData(new[] { (long)int.MaxValue + 3000, long.MaxValue })]
        [InlineData(new[] { -byte.MaxValue, (long)(int.MaxValue) + 10 })]
        [InlineData(new[] { -byte.MaxValue, (long)(int.MaxValue) + 200 })]
        [InlineData(new[] { -byte.MaxValue, (long)(int.MaxValue) + 3000 })]
        [InlineData(new[] { long.MinValue + byte.MaxValue, byte.MaxValue })]
        public void CreateReturnsNumberInCorrectRangeForInt64OnMultipleCall(long[] limits)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            long expectedMin = limits.Min();
            long expectedMax = limits.Max();
            int repeatCount = 300;
            var sut = new RandomNumericSequenceGenerator(expectedMin, expectedMax);
            // Act
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(long), dummyContext))
                .Cast<long>();
            // Assert
            Assert.True(
                result.All(x => x >= expectedMin && x <= expectedMax)
                );
        }

        [Theory]
        [InlineData(new long[] { -30, -9, -5, 2, 9, 15 }, 0, 1)]
        [InlineData(new long[] { -30, -9, -5, 2, 9, 15 }, 1, 2)]
        [InlineData(new long[] { -30, -9, -5, 2, 9, 15 }, 2, 3)]
        [InlineData(new long[] { -30, -9, -5, 2, 9, 15 }, 3, 4)]
        [InlineData(new long[] { -30, -9, -5, 2, 9, 15 }, 4, 5)]
        [InlineData(new long[] { 1, 5, 9, 30, 128, 255 }, 0, 1)]
        [InlineData(new long[] { 1, 5, 9, 30, 128, 255 }, 1, 2)]
        [InlineData(new long[] { 1, 5, 9, 30, 128, 255 }, 2, 3)]
        [InlineData(new long[] { 1, 5, 9, 30, 128, 255 }, 3, 5)]
        [InlineData(new long[] { 1, 5, 9, 30, 128, 255 }, 4, 5)]
        public void CreateReturnsNumberInCorrectRangeProgressivelyOnMultipleCall(
            long[] limits, int indexOfLower, int indexOfUpper)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator(limits);
            var expectedMin = (int)limits[indexOfLower];
            var expectedMax = (int)limits[indexOfUpper];
            int repeatCount = expectedMax - expectedMin;
            int lowerBounds = Math.Abs((int)limits.Min() + expectedMin);
            // Act
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .ToArray()
                .Skip(lowerBounds)
                .Cast<int>();
            // Assert
            Assert.True(result.All(x => x >= expectedMin && x <= expectedMax));
        }

        [Theory]
        [MemberData(nameof(LimitSequenceTestCases))]
        public void CreateWhenLimitIsReachedStartsFromTheBeginning(long[] limits)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var min = (int)limits.Min();
            var max = (int)limits.Max();
            int repeatCount = (max - min) + 1;
            var sut = new RandomNumericSequenceGenerator(limits);
            // Act & assert
            var expectedResult = Enumerable.Range(min, repeatCount).ToArray();
            for (int iteration = 0; iteration < 3; iteration++)
            {
                var result = Enumerable
                    .Range(0, repeatCount)
                    .Select(i => sut.Create(typeof(int), dummyContext))
                    .Cast<int>()
                    .OrderBy(x => x);

                Assert.True(expectedResult.SequenceEqual(result));
            }
        }

        [Theory]
        [MemberData(nameof(LimitSequenceTestCases))]
        public void CreateReturnsUniqueNumbersOnMultipleCall(long[] limits)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var min = (int)limits.Min();
            var max = (int)limits.Max();
            int repeatCount = (max - min) + 1;
            var sut = new RandomNumericSequenceGenerator(limits);
            // Act
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .Cast<int>()
                .OrderBy(x => x);
            // Assert
            var expectedResult = Enumerable.Range(min, repeatCount);
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Theory]
        [MemberData(nameof(LimitSequenceTestCases))]
        public void CreateReturnsUniqueNumbersOnMultipleCallAsynchronously(long[] limits)
        {
            // Arrange
            int iterations = 5;
            int completed = 0;
            var done = new ManualResetEvent(false);
            var min = (int)limits.Min();
            var max = (int)limits.Max();
            int repeatCount = ((max - min) + 1) / iterations;
            int expectedResult = repeatCount * iterations;
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator(limits);
            // Act
            var numbers = new int[iterations][];
            for (int i = 0; i < iterations; i++)
            {
                ThreadPool.QueueUserWorkItem(index =>
                {
                    numbers[(int)index] =
                        Enumerable
                            .Range(0, repeatCount)
                            .Select(x => sut.Create(typeof(int), dummyContext))
                            .Cast<int>()
                            .ToArray();

                    if (Interlocked.Increment(ref completed) == iterations)
                        done.Set();
                }, i);
            }
            done.WaitOne();
            int result = numbers.SelectMany(x => x).Distinct().Count();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static TheoryData<long[]> LimitSequenceTestCases =>
            new TheoryData<long[]>
            {
                new long[] { 2, 5, 9, 30, 255 },
                new long[] { 2, 5, 9, 30, 255, 512 },
                new long[] { -30, -9, -5, 2, 5, 9, 30 }
            };
    }
}
