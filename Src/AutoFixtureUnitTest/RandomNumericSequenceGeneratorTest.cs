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
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var dummyRequest = typeof(byte);
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new NoSpecimen(request);
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWhenContextReturnValueIsNullReturnsCorrectResult()
        {
            // Fixture setup
            object expectedContextValue = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? expectedContextValue : new NoSpecimen(r)
            };
            var dummyRequest = typeof(int);
            var expectedResult = new NoSpecimen(dummyRequest);
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var result = sut.Create(dummyRequest, context);
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
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? (object)new RandomNumericSequenceLimit() : new NoSpecimen(r)
            };
            var expectedResult = new NoSpecimen(request);
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(request, context);
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
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? (object)new RandomNumericSequenceLimit() : new NoSpecimen(r)
            };
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType(request, result);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(UpperLimitSequenceTestCases))]
        public void CreateReturnsNumberInCorrectRange(int[] expectedUpperLimits)
        {
            // Fixture setup
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? (object)new RandomNumericSequenceLimit(expectedUpperLimits) : new NoSpecimen(r)
            };
            var dummyRequest = typeof(double);
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var result = (double)sut.Create(dummyRequest, context);
            // Verify outcome
            Assert.True(
                result >= 1 && result < expectedUpperLimits.Max()
                );
            // Teardown
        }

        [Theory]
        [ClassData(typeof(UpperLimitSequenceTestCases))]
        public void CreateReturnsNumbersInCorrectRangeOnMultipleCall(int[] upperLimits)
        {
            // Fixture setup
            object contextResult = new RandomNumericSequenceLimit(upperLimits);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? contextResult : new NoSpecimen(r)
            };
            int repeatCount = upperLimits.Max();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), context))
                .Cast<int>();
            // Verify outcome
            Assert.True(
                result.All(x => x >= 1 && x <= upperLimits.Max())
                );
            // Teardown
        }

        [Theory]
        [ClassData(typeof(UpperLimitSequenceTestCases))]
        public void CreateWhenPassTheUpperLimitThrowsOnMultipleCall(int[] upperLimits)
        {
            // Fixture setup
            object contextResult = new RandomNumericSequenceLimit(upperLimits);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? contextResult : new NoSpecimen(r)
            };
            int repeatCount = upperLimits.Max() + 1;
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                Enumerable
                    .Range(0, repeatCount)
                    .Select(i => sut.Create(typeof(int), context))
                    .ToList()
                );
            // Teardown
        }

        [Theory]
        [ClassData(typeof(UpperLimitSequenceTestCases))]
        public void CreateReturnsAsManyNumbersAsTheUpperLimitOnMultipleCall(int[] upperLimits)
        {
            // Fixture setup
            object contextResult = new RandomNumericSequenceLimit(upperLimits);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? contextResult : new NoSpecimen(r)
            };
            int expectedCount = upperLimits.Max();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var result = Enumerable
                .Range(0, expectedCount)
                .Select(i => sut.Create(typeof(int), context))
                .Cast<int>();
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

        [Theory]
        [ClassData(typeof(UpperLimitSequenceTestCases))]
        public void CreateReturnsUniqueNumbersOnMultipleCall(int[] upperLimits)
        {
            // Fixture setup
            object contextResult = new RandomNumericSequenceLimit(upperLimits);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? contextResult : new NoSpecimen(r)
            };
            int repeatCount = upperLimits.Max();
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var result = Enumerable
                .Range(0, repeatCount)
                .Select(i => sut.Create(typeof(int), context))
                .Cast<int>()
                .OrderBy(x => x);
            // Verify outcome
            var expectedResult = Enumerable.Range(1, repeatCount);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [ClassData(typeof(UpperLimitSequenceTestCases))]
        public void CreateReturnsUniqueNumbersOnMultipleCallAsynchronously(int[] upperLimits)
        {
            // Fixture setup
            object contextResult = new RandomNumericSequenceLimit(upperLimits);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(RandomNumericSequenceLimit).Equals(r) ? contextResult : new NoSpecimen(r)
            };
            int iterations = 5;
            int completed = 0;
            var done = new ManualResetEvent(false);
            int repeatCount = upperLimits.Max() / iterations;
            int expectedResult = repeatCount * iterations;
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var numbers = new List<int[]>();
            for (int i = 0; i < iterations; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    numbers.Add(
                        Enumerable
                            .Range(0, repeatCount)
                            .Select(x => sut.Create(typeof(int), context))
                            .Cast<int>()
                            .ToArray());

                    if (Interlocked.Increment(ref completed) == iterations)
                        done.Set();
                });
            }
            done.WaitOne();
            int result = numbers.SelectMany(x => x).Distinct().Count();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private sealed class UpperLimitSequenceTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new[] { 2, 5, 9, 30 } };
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