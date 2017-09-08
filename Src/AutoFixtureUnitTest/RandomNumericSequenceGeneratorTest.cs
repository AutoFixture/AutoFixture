using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomNumericSequenceGeneratorTest
    {
        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => new RandomNumericSequenceGenerator()));
            // Teardown
        }

        [Fact]
        public void InitializeWithDefaultConstructorSetsCorrectLimits()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            var expectedResult = new long[]
            {
                1,
                Byte.MaxValue,
                Int16.MaxValue,
                Int32.MaxValue
            };
            // Exercise system
            IEnumerable<long> result = sut.Limits;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            IEnumerable<long> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullEnumerable));
            // Teardown
        }

        [Theory]
        [InlineData(new long[] { 1 })]
        [InlineData(new long[] { 20 })]
        [InlineData(new long[] { 300 })]
        public void InitializeWithSingleLimitThrows(long[] limits)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new RandomNumericSequenceGenerator(limits));
            // Teardown
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
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RandomNumericSequenceGenerator(limits));
            // Teardown
        }

        [Fact]
        public void LimitsMatchListParameter()
        {
            // Fixture setup
            var expectedResult = new long[] { 10, 20, 30 }.AsEnumerable();
            var sut = new RandomNumericSequenceGenerator(expectedResult);
            // Exercise system
            IEnumerable<long> result = sut.Limits;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            long[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullArray));
            // Teardown
        }

        [Fact]
        public void LimitsMatchParamsArray()
        {
            // Fixture setup
            var expectedResult = new long[] { 10, 20, 30 };
            var sut = new RandomNumericSequenceGenerator(expectedResult);
            // Exercise system
            IEnumerable<long> result = sut.Limits;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomNumericSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var dummyRequest = typeof(byte);
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new NoSpecimen();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(bool))]
        public void CreateWithNonNumericTypeRequestReturnsNoSpecimen(Type request)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new NoSpecimen();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
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
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.IsType(request, result);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(LimitSequenceTestCases))]
        public void CreateReturnsNumberInCorrectRange(long[] limits)
        {
            // Fixture setup
            var request = typeof(double);
            var dummyContext = new DelegatingSpecimenContext();
            var expectedMin = (int)limits.Min();
            var expectedMax = (int)limits.Max();
            var sut = new RandomNumericSequenceGenerator(limits);
            // Exercise system
            var result = (double)sut.Create(request, dummyContext);
            // Verify outcome
            Assert.True(
                result >= expectedMin && result <= expectedMax
                );
            // Teardown
        }

        [Theory]
        [ClassData(typeof(LimitSequenceTestCases))]
        public void CreateReturnsNumberInCorrectRangeOnMultipleCall(long[] limits)
        {
            // Fixture setup
            var request = typeof(int);
            var dummyContext = new DelegatingSpecimenContext();
            var expectedMin = (int)limits.Min();
            var expectedMax = (int)limits.Max();
            int repeatCount = (expectedMax - expectedMin) + 1;
            var sut = new RandomNumericSequenceGenerator(limits);
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(request, dummyContext))
                .Cast<int>();
            // Verify outcome
            Assert.True(
                result.All(x => x >= expectedMin && x <= expectedMax)
                );
            // Teardown
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
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            long expectedMin = limits.Min();
            long expectedMax = limits.Max();
            int repeatCount = 300;
            var sut = new RandomNumericSequenceGenerator(expectedMin, expectedMax);
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(long), dummyContext))
                .Cast<long>();
            // Verify outcome
            Assert.True(
                result.All(x => x >= expectedMin && x <= expectedMax)
                );
            // Teardown
        }

        [Theory]
        [InlineData(new object[] { new long[] { -30, -9, -5, 2, 9, 15 }, 0, 1 })]
        [InlineData(new object[] { new long[] { -30, -9, -5, 2, 9, 15 }, 1, 2 })]
        [InlineData(new object[] { new long[] { -30, -9, -5, 2, 9, 15 }, 2, 3 })]
        [InlineData(new object[] { new long[] { -30, -9, -5, 2, 9, 15 }, 3, 4 })]
        [InlineData(new object[] { new long[] { -30, -9, -5, 2, 9, 15 }, 4, 5 })]
        [InlineData(new object[] { new long[] { 1, 5, 9, 30, 128, 255 }, 0, 1 })]
        [InlineData(new object[] { new long[] { 1, 5, 9, 30, 128, 255 }, 1, 2 })]
        [InlineData(new object[] { new long[] { 1, 5, 9, 30, 128, 255 }, 2, 3 })]
        [InlineData(new object[] { new long[] { 1, 5, 9, 30, 128, 255 }, 3, 5 })]
        [InlineData(new object[] { new long[] { 1, 5, 9, 30, 128, 255 }, 4, 5 })]
        public void CreateReturnsNumberInCorrectRangeProgressivelyOnMultipleCall(
            long[] limits, int indexOfLower, int indexOfUpper)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator(limits);
            var expectedMin = (int)limits[indexOfLower];
            var expectedMax = (int)limits[indexOfUpper];
            int repeatCount = expectedMax - expectedMin;
            int lowerBounds = Math.Abs((int)limits.Min() + expectedMin);
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .ToArray()
                .Skip(lowerBounds)
                .Cast<int>();
            // Verify outcome
            Assert.True(result.All(x => x >= expectedMin && x <= expectedMax));
            // Teardown
        }

        [Theory]
        [ClassData(typeof(LimitSequenceTestCases))]
        public void CreateWhenLimitIsReachedStartsFromTheBeginning(long[] limits)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var min = (int)limits.Min();
            var max = (int)limits.Max();
            int repeatCount = (max - min) + 1;
            var sut = new RandomNumericSequenceGenerator(limits);
            // Exercise system and verify outcome
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
            // Teardown
        }

        [Theory]
        [ClassData(typeof(LimitSequenceTestCases))]
        public void CreateReturnsUniqueNumbersOnMultipleCall(long[] limits)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var min = (int)limits.Min();
            var max = (int)limits.Max();
            int repeatCount = (max - min) + 1;
            var sut = new RandomNumericSequenceGenerator(limits);
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .Cast<int>()
                .OrderBy(x => x);
            // Verify outcome
            var expectedResult = Enumerable.Range(min, repeatCount);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [ClassData(typeof(LimitSequenceTestCases))]
        public void CreateReturnsUniqueNumbersOnMultipleCallAsynchronously(long[] limits)
        {
            // Fixture setup
            int iterations = 5;
            int completed = 0;
            var done = new ManualResetEvent(false);
            var min = (int)limits.Min();
            var max = (int)limits.Max();
            int repeatCount = ((max - min) + 1) / iterations;
            int expectedResult = repeatCount * iterations;
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceGenerator(limits);
            // Exercise system
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
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private sealed class LimitSequenceTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new long[] { 2, 5, 9, 30, 255 } };
                yield return new object[] { new long[] { 2, 5, 9, 30, 255, 512 } };
                yield return new object[] { new long[] { -30, -9, -5, 2, 5, 9, 30 } };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
