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
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            IEnumerable<int> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullEnumerable));
            // Teardown
        }

        [Fact]
        public void BoundariesMatchListParameter()
        {
            // Fixture setup
            IEnumerable<int> expectedBoundaries = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue 
            }.AsEnumerable();
            var sut = new RandomNumericSequenceGenerator(expectedBoundaries);
            // Exercise system
            IEnumerable<int> result = sut.Boundaries;
            // Verify outcome
            Assert.True(expectedBoundaries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            int[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullArray));
            // Teardown
        }

        [Fact]
        public void BoundariesMatchParamsArray()
        {
            // Fixture setup
            int[] expectedBoundaries = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue
            };
            var sut = new RandomNumericSequenceGenerator(expectedBoundaries);
            // Exercise system
            IEnumerable<int> result = sut.Boundaries;
            // Verify outcome
            Assert.True(expectedBoundaries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void BoundariesDoesNotMatchParamsArray()
        {
            // Fixture setup
            int[] expectedBoundaries = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue
            };
            var sut = new RandomNumericSequenceGenerator(
                expectedBoundaries[0],
                expectedBoundaries[2],
                expectedBoundaries[1]
                );
            // Exercise system
            IEnumerable<int> result = sut.Boundaries;
            // Verify outcome
            Assert.False(expectedBoundaries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            object result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.DoesNotThrow(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            var expectedResult = new NoSpecimen(request);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
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
            var sut = new RandomNumericSequenceGenerator();
            var expectedResult = new NoSpecimen(request);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
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
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            object result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.IsType(request, result);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(RandomNumericSequenceBoundariesTestCases))]
        public void CreateMultipleTimesWhenPassTheUpperBoundWillThrow(int[] boundaries)
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator(boundaries);
            int boundFromAbove = boundaries.Max() + 1;
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                Enumerable
                    .Range(0, boundFromAbove)
                    .Select(i => sut.Create(typeof(int), dummyContext))
                    .ToList()
                );
            // Teardown
        }

        [Theory]
        [ClassData(typeof(RandomNumericSequenceBoundariesTestCases))]
        public void CreateMultipleTimesReturnsNumbersInBoundaries(int[] boundaries)
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator(boundaries);
            int repeatCount = boundaries.Max();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .Cast<int>();
            // Verify outcome
            Assert.True(
                result.All(x => x >= 1 && x <= boundaries.Max())
                );
            // Teardown
        }

        [Theory]
        [ClassData(typeof(RandomNumericSequenceBoundariesTestCases))]
        public void CreateMultipleTimesReturnsAsManyNumbersAsTheUpperBound(int[] boundaries)
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator(boundaries);
            int expectedCount = boundaries.Max();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = Enumerable
                .Range(0, expectedCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .Cast<int>();
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

        [Theory]
        [ClassData(typeof(RandomNumericSequenceBoundariesTestCases))]
        public void CreateMultipleTimesReturnsUniqueNumbers(int[] boundaries)
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator(boundaries);
            int repeatCount = boundaries.Max();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), dummyContext))
                .Cast<int>()
                .OrderBy(x => x);
            // Verify outcome
            var expectedResult = Enumerable.Range(1, repeatCount);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [ClassData(typeof(RandomNumericSequenceBoundariesTestCases))]
        public void CreateAsynchronouslyMultipleTimesReturnsUniqueNumbers(int[] boundaries)
        {
            // Fixture setup
            const int iterations = 5;
            int completed = 0;
            var done = new ManualResetEvent(false);
            int repeatCount = boundaries.Max() / iterations;
            var sut = new RandomNumericSequenceGenerator(boundaries);
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var numbers = new List<int[]>();
            for (int i = 0; i < iterations; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    numbers.Add(
                        Enumerable
                            .Range(0, repeatCount)
                            .Select(x => sut.Create(typeof(int), dummyContext))
                            .Cast<int>()
                            .ToArray());

                    if (Interlocked.Increment(ref completed) == iterations)
                        done.Set();
                });
            }
            done.WaitOne();
            int result = numbers.SelectMany(x => x).Distinct().Count();
            // Verify outcome
            int expectedResult = repeatCount * iterations;
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private sealed class RandomNumericSequenceBoundariesTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new[] { 2, 5, 9, 30} };
                yield return new object[] { new[] { 2, 5, 9, 30, 255 } };
                yield return new object[] { new[] { 2, 5, 9, 30, 255, 512 } };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}