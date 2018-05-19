using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomRangedNumberGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RandomRangedNumberGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RandomRangedNumberGenerator();
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
            var sut = new RandomRangedNumberGenerator();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new RandomRangedNumberGenerator();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Theory]
        [InlineData(typeof(int), "a", "b")]
        [InlineData(typeof(long), 'd', 'e')]
        [InlineData(typeof(double), 'f', 'g')]
        [InlineData(typeof(decimal), 'h', 'j')]
        public void CreateWithNonnumericLimitsReturnsNoSpecimen(Type operandType, object minimum, object maximum)
        {
            // Arrange
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            var request = new RangedNumberRequest(operandType, minimum, maximum);
            // Act
            var result = sut.Create(request, dummyContext);
            // Verify
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateReturnsAllValuesInSetBeforeRepeating()
        {
            // Arrange
            int minimum = 0;
            int maximum = 2;

            var request = new RangedNumberRequest(typeof(int), minimum, maximum);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Act
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
            // Arrange
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

            // Act
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
            // Arrange
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Act
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
            // Arrange
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);
            var request3 = new RangedNumberRequest(typeof(int), 0, 2);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Act
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
        [MemberData(nameof(PairsOfDifferentIntegerTypes))]
        public void CreateReturnsValuesFromCorrectSetForRequestsWithDifferentTypesAndSameLimits(
                                                                      Type primaryRequestType, Type otherRequestType)
        {
            // Arrange
            int minimum = 0, maximum = 2;
            int rangeCount = maximum - minimum + 1;

            var primaryRequest = new RangedNumberRequest(primaryRequestType, minimum, maximum);
            var otherRequest = new RangedNumberRequest(otherRequestType, minimum, maximum);
            var dummyContext = new DelegatingSpecimenContext();
            var primaryResults = new List<IComparable>();

            var sut = new RandomRangedNumberGenerator();

            // Act
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
                      .All(a => primaryResults.Any(b => b.CompareTo(a) == 0)));
        }

        [Theory]
        [InlineData(1, 50000, 5)]
        [InlineData(-50000, -1, 5)]
        [InlineData(-25000, 24999, 5)]
        public void CreateReturnsUniqueNumbersOnMultipleCallAsynchronously(int minimum, int maximum, int numberOfThreads)
        {
            // Arrange
            int expectedDistinctCount = Math.Abs((maximum - minimum + 1));
            int requestsPerThread = expectedDistinctCount / numberOfThreads;
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Act

            var numbers = Enumerable
                .Range(0, numberOfThreads)
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(numberOfThreads)
                .Select(threadNumber => Enumerable
                                        .Range(0, requestsPerThread)
                                        .Select(_ => new RangedNumberRequest(typeof(int), minimum, maximum))
                                        .Select(request => sut.Create(request, dummyContext))
                                        .Cast<int>()
                                        .ToArray())
                .ToArray();

            // Verify
            int actualDistinctCount = numbers.SelectMany(a => a).Distinct().Count();
            Assert.Equal(expectedDistinctCount, actualDistinctCount);

        }

        [Theory]
        [MemberData(nameof(MinLimitToMaxLimitRequests))]
        public void CreationOnFullRangeShouldNotFail(Type type, object minimum, object maximum)
        {
            // Arrange
            var request = new RangedNumberRequest(type, minimum, maximum);
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();

            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Create(request, dummyContext)));

        }

        [Theory]
        [MemberData(nameof(MinLimitToMaxLimitRequests))]
        public void CreationOnFullRangeShouldReturnValue(Type type, object minimum, object maximum)
        {
            // Arrange
            var request = new RangedNumberRequest(type, minimum, maximum);
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType(type, result);

        }

        [Theory]
        [MemberData(nameof(RequestsWithLimitsToZeroRange))]
        public void CreationWithLimitsInBoundariesShouldReturnValueInRange(Type type, object minimum, object maximum)
        {
            // Arrange
            var request = new RangedNumberRequest(type, minimum, maximum);
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = (IComparable)sut.Create(request, dummyContext);

            // Assert
            Assert.InRange(result, (IComparable)minimum, (IComparable)maximum);

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
        public void ShouldCorrectlyHandleRequestsWithSameMinimumAndMaximumValue(Type type)
        {
            // Arrange
            var range = Convert.ChangeType(42, type, CultureInfo.InvariantCulture);
            var sut = new RandomRangedNumberGenerator();
            var request = new RangedNumberRequest(type, range, range);
            var dummyContext = new DelegatingSpecimenContext();

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.Equal(range, result);
        }

        public static TheoryData<Type, IConvertible, IConvertible> MinLimitToMaxLimitRequests =>
            new TheoryData<Type, IConvertible, IConvertible>
            {
                { typeof(float), float.MinValue, float.MaxValue },
                { typeof(double), double.MinValue, double.MaxValue },
                { typeof(decimal), decimal.MinValue, decimal.MaxValue },
                { typeof(sbyte), sbyte.MinValue, sbyte.MaxValue },
                { typeof(byte), byte.MinValue, byte.MaxValue },
                { typeof(short), short.MinValue, short.MaxValue },
                { typeof(ushort), ushort.MinValue, ushort.MaxValue },
                { typeof(int), int.MinValue, int.MaxValue },
                { typeof(uint), uint.MinValue, uint.MaxValue },
                { typeof(long), long.MinValue, long.MaxValue },
                { typeof(ulong), ulong.MinValue, ulong.MaxValue }
            };

        public static TheoryData<Type, IConvertible, IConvertible> RequestsWithLimitsToZeroRange =>
            new TheoryData<Type, IConvertible, IConvertible>
            {
                { typeof(float), float.MinValue, (float)0 },
                { typeof(float), (float)0, float.MaxValue },

                { typeof(double), double.MinValue, (double)0 },
                { typeof(double), (double)0, double.MaxValue },

                { typeof(decimal), decimal.MinValue, (decimal)0 },
                { typeof(decimal), (decimal)0, decimal.MaxValue },

                { typeof(sbyte), sbyte.MinValue, (sbyte)0 },
                { typeof(sbyte), (sbyte)0, sbyte.MaxValue },
                { typeof(byte), (byte)0, byte.MaxValue },

                { typeof(short), short.MinValue, (short)0 },
                { typeof(short), (short)0, short.MaxValue },
                { typeof(ushort), (ushort)0, ushort.MaxValue },

                { typeof(int), int.MinValue, (int)0 },
                { typeof(int), (int)0, int.MaxValue },
                { typeof(uint), (uint)0, uint.MaxValue },

                { typeof(long), long.MinValue, (long)0 },
                { typeof(long), (long)0, long.MaxValue },
                { typeof(ulong), (ulong)0, ulong.MaxValue }
            };

        public static TheoryData<Type, Type> PairsOfDifferentIntegerTypes =>
            new TheoryData<Type, Type>
            {
                { typeof(sbyte), typeof(int) },
                { typeof(sbyte), typeof(byte) },
                { typeof(sbyte), typeof(short) },
                { typeof(sbyte), typeof(long) },
                { typeof(sbyte), typeof(ulong) },
                { typeof(sbyte), typeof(ushort) },

                { typeof(long), typeof(int) },
                { typeof(long), typeof(byte) },
                { typeof(long), typeof(short) },
                { typeof(long), typeof(sbyte) },
                { typeof(long), typeof(ushort) },
                { typeof(long), typeof(uint) },

                { typeof(int), typeof(ulong) },
                { typeof(int), typeof(byte) },
                { typeof(int), typeof(short) },
                { typeof(int), typeof(long) },
                { typeof(int), typeof(ushort) },
                { typeof(int), typeof(sbyte) },

                { typeof(short), typeof(int) },
                { typeof(short), typeof(byte) },
                { typeof(short), typeof(ushort) },
                { typeof(short), typeof(long) },
                { typeof(short), typeof(sbyte) },
                { typeof(short), typeof(ulong) },

                { typeof(byte), typeof(int) },
                { typeof(byte), typeof(short) },
                { typeof(byte), typeof(sbyte) },
                { typeof(byte), typeof(long) },
                { typeof(byte), typeof(ushort) },
                { typeof(byte), typeof(ulong) },

                { typeof(uint), typeof(int) },
                { typeof(uint), typeof(short) },
                { typeof(uint), typeof(sbyte) },
                { typeof(uint), typeof(long) },
                { typeof(uint), typeof(ulong) },
                { typeof(uint), typeof(byte) }
            };
    }
}
