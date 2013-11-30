﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomRangedNumberGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomRangedNumberGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RandomRangedNumberGenerator();
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
            var sut = new RandomRangedNumberGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RandomRangedNumberGenerator();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(dummyRequest), result);
            // Teardown
        }


        [Theory]
        [InlineData(typeof(int), "a", "b")]
        [InlineData(typeof(long), 'd', 'e')]
        public void CreateWithNonnumericLimitsReturnsNoSpecimen(Type operandType, object minimum, object maximum)
        {
            // Fixture setup
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            var request = new RangedNumberRequest(operandType, minimum, maximum);
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify
            Assert.Equal(new NoSpecimen(request), result);
        }


        [Fact]
        public void CreateReturnsAllValuesInSetBeforeRepeating()
        {
            // Fixture setup
            int minimum = 0;
            int maximum = 2;

            var request = new RangedNumberRequest(typeof(int), minimum, maximum);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            var generatedValues = Enumerable.Range(0, 3).Select(i => sut.Create(request, dummyContext)).Cast<int>();
            var shouldBeRepeatedValue = (int)sut.Create(request, dummyContext);

            // Verify
            Assert.True(generatedValues.All(a => a >= minimum && a <= maximum));
            Assert.InRange(shouldBeRepeatedValue, minimum, maximum);

        }

        [Theory]
        [InlineData(0, 2, 1, 2, 0, 3, 2, 2, 0, 4, 2, 3)]
        [InlineData(-20, 10, 20, 11, -20, 15, 21, 15, -20, 20, 21, 20)]
        [InlineData(0, 2, 1, 2, -1, 2, 2, 2, -2, 2, 2, 3)]
        [InlineData(-20, 10, 20, 11, -15, 10, 16, 10, -10, 10, 11, 10)]
        public void CreateReturnsCorrectValuesForRequestsOfSameTypeAndSingleSharedLimit
            (int request1Min, int request1Max, int request1FirstGroup, int request1SecondGroup,
            int request2Min, int request2Max, int request2FirstGroup, int request2SecondGroup,
            int request3Min, int request3Max, int request3FirstGroup, int request3SecondGroup)
        {
            // Fixture setup
            int request1Count = request1Max - request1Min + 1;
            int request2Count = request2Max - request2Min + 1;
            int request3Count = request3Max - request3Min + 1;

            var request1 = new RangedNumberRequest(typeof(int), request1Min, request1Max);
            var request2 = new RangedNumberRequest(typeof(int), request2Min, request2Max);
            var request3 = new RangedNumberRequest(typeof(int), request3Min, request3Max);

            IDictionary<RangedNumberRequest, IList<int>> results = new Dictionary<RangedNumberRequest, IList<int>>()
            {
                { request1, new List<int>() }, { request2, new List<int>() }, { request3, new List<int>() }
            };

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            results[request1].AddMany(() => (int)sut.Create(request1, dummyContext), request1FirstGroup);
            results[request2].AddMany(() => (int)sut.Create(request2, dummyContext), request2FirstGroup);
            results[request3].AddMany(() => (int)sut.Create(request3, dummyContext), request3FirstGroup);
            results[request1].AddMany(() => (int)sut.Create(request1, dummyContext), request1SecondGroup);
            results[request2].AddMany(() => (int)sut.Create(request2, dummyContext), request2SecondGroup);
            results[request3].AddMany(() => (int)sut.Create(request3, dummyContext), request3SecondGroup);

            // Verify           
            Assert.True(Enumerable.Range(request1Min, request1Count)
                                  .Intersect(results[request1]).Count() == request1Count);
            Assert.True(Enumerable.Range(request2Min, request2Count)
                                  .Intersect(results[request2]).Count() == request2Count);
            Assert.True(Enumerable.Range(request3Min, request3Count)
                                  .Intersect(results[request3]).Count() == request3Count); 
        }
              

        [Fact]
        public void CreateReturnsValuesFromCorrectSetForTwoRequestsOfSameTypeAndDifferentLimits()
        {
            // Fixture setup
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            var value1 = (int)sut.Create(request1, dummyContext);
            var value2 = (int)sut.Create(request2, dummyContext);
            var value3 = (int)sut.Create(request1, dummyContext);

            // Verify
            Assert.InRange(value1, 0, 2);
            Assert.InRange(value2, 10, 20);
            Assert.InRange(value3, 0, 2);
        }

        [Fact]
        public void CreateReturnsValuesFromCorrectSetForMultipleRequestsWithInterspersedDifferentRequestOfSameType()
        {
            // Fixture setup
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);
            var request3 = new RangedNumberRequest(typeof(int), 0, 2);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            var value1 = (int)sut.Create(request1, dummyContext);
            var value2 = (int)sut.Create(request2, dummyContext);
            var value3 = (int)sut.Create(request3, dummyContext);

            // Verify
            Assert.InRange(value1, 0, 2);
            Assert.InRange(value2, 10, 20);
            Assert.InRange(value3, 0, 2);
            Assert.NotEqual(value1, value3);
        }


        [Theory]
        [ClassData(typeof(RandomRangedNumberGeneratorTestCases))]
        public void CreateReturnsValuesFromCorrectSetForRequestsWithDifferentTypesAndSameLimits(
                                                                      Type primaryRequestType, Type otherRequestType)
        {
            // Fixture setup          
            int minimum = 0, maximum = 2;
            int rangeCount = maximum - minimum + 1;

            var primaryRequest = new RangedNumberRequest(primaryRequestType, minimum, maximum);
            var otherRequest = new RangedNumberRequest(otherRequestType, minimum, maximum);
            var dummyContext = new DelegatingSpecimenContext();
            var primaryResults = new List<IComparable>();

            var sut = new RandomRangedNumberGenerator();
            
            // Exercise system
            primaryResults.Add((IComparable)sut.Create(primaryRequest, dummyContext));
            primaryResults.Add((IComparable)sut.Create(primaryRequest, dummyContext));
            var otherResult = ((IComparable)sut.Create(otherRequest, dummyContext));             
            primaryResults.Add((IComparable)sut.Create(primaryRequest, dummyContext));

            // Verify
            Assert.True(primaryResults.Distinct().Count() == 3);
            Assert.InRange(otherResult, minimum, maximum);              

            Assert.True(Enumerable.Range(minimum, rangeCount)
                      .Select(a => Convert.ChangeType(a, primaryRequestType))
                      .Cast<IComparable>()
                      .All(a => primaryResults.Any(b => b.CompareTo(a)==0)));
        }

        [Theory]
        [InlineData(0, 500, 100)]
        [InlineData(-20000, 0, 50)]
        [InlineData(-300, 350, 25)]
        public void CreateReturnsUniqueNumbersOnMultipleCallAsynchronously(int minimum, int maximum, int timesToTry)
        {
            // Fixture setup          
            int tryCount = 0;           

            while (tryCount++ < timesToTry)
            {

                int completed = 0;
                int iterations = 25;

                var done = new ManualResetEvent(false);

                int repeatCount = ((maximum - minimum) + 1) / iterations;
                int expectedResult = repeatCount * iterations;
                var dummyContext = new DelegatingSpecimenContext();

                int exceptionCount = 0;

                var sut = new RandomRangedNumberGenerator();

                // Exercise system
                var numbers = new int[iterations][];
                for (int i = 0; i < iterations; i++)
                {
                    ThreadPool.QueueUserWorkItem(index =>
                    {
                        var request = new RangedNumberRequest(typeof(int), minimum, maximum);

                        try
                        {
                            numbers[(int)index] =
                                Enumerable
                                    .Range(0, repeatCount)
                                    .Select(x => sut.Create(request, dummyContext))
                                    .Cast<int>()
                                    .ToArray();

                            if (Interlocked.Increment(ref completed) == iterations)
                                done.Set();
                        }
                        catch (Exception)
                        {
                            Interlocked.Increment(ref exceptionCount);
                            done.Set();
                        }

                    }, i);
                }

                done.WaitOne();

                Assert.True(exceptionCount == 0, "Thread-safety failed - exception thrown by worker thread");
                int result = numbers.SelectMany(x => x).Distinct().Count();
                Assert.Equal(expectedResult, result);
            }
            // Nothing else to verify
            // Teardown      
        }


        private sealed class RandomRangedNumberGeneratorTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { typeof(double), typeof(int) };
                yield return new object[] { typeof(double), typeof(byte) };
                yield return new object[] { typeof(double), typeof(short) };
                yield return new object[] { typeof(double), typeof(long) };
                yield return new object[] { typeof(double), typeof(float) };
                yield return new object[] { typeof(double), typeof(decimal) };

                yield return new object[] { typeof(float), typeof(int) };
                yield return new object[] { typeof(float), typeof(byte) };
                yield return new object[] { typeof(float), typeof(short) };
                yield return new object[] { typeof(float), typeof(long) };
                yield return new object[] { typeof(float), typeof(double) };
                yield return new object[] { typeof(float), typeof(decimal) };


                yield return new object[] { typeof(int), typeof(float) };
                yield return new object[] { typeof(int), typeof(byte) };
                yield return new object[] { typeof(int), typeof(short) };
                yield return new object[] { typeof(int), typeof(long) };
                yield return new object[] { typeof(int), typeof(double) };
                yield return new object[] { typeof(int), typeof(decimal) };

                yield return new object[] { typeof(short), typeof(int) };
                yield return new object[] { typeof(short), typeof(byte) };
                yield return new object[] { typeof(short), typeof(float) };
                yield return new object[] { typeof(short), typeof(long) };
                yield return new object[] { typeof(short), typeof(double) };
                yield return new object[] { typeof(short), typeof(decimal) };

                yield return new object[] { typeof(byte), typeof(int) };
                yield return new object[] { typeof(byte), typeof(short) };
                yield return new object[] { typeof(byte), typeof(float) };
                yield return new object[] { typeof(byte), typeof(long) };
                yield return new object[] { typeof(byte), typeof(double) };
                yield return new object[] { typeof(byte), typeof(decimal) };

                yield return new object[] { typeof(decimal), typeof(int) };
                yield return new object[] { typeof(decimal), typeof(short) };
                yield return new object[] { typeof(decimal), typeof(float) };
                yield return new object[] { typeof(decimal), typeof(long) };
                yield return new object[] { typeof(decimal), typeof(double) };
                yield return new object[] { typeof(decimal), typeof(byte) };               
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }     

    }
}