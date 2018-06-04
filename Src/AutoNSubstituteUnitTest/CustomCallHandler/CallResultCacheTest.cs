using System;
using System.Linq;
using AutoFixture.AutoNSubstitute.CustomCallHandler;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultCacheTest
    {
        [Fact]
        public void AddedResultShouldBeReturned()
        {
            // Arrange
            var sut = new CallResultCache();
            var cachedResult = new CallResultData(Maybe.Nothing<object>(), null);

            var call = Substitute.For<ICall>();
            var callSpec = Substitute.For<ICallSpecification>();
            callSpec.IsSatisfiedBy(call).Returns(true);

            // Act
            sut.AddResult(callSpec, cachedResult);
            CallResultData retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            // Assert
            Assert.True(hasResult);
            Assert.Same(cachedResult, retrievedResult);
        }

        [Fact]
        public void TheLatestResultShouldBeReturned()
        {
            // Arrange
            var sut = new CallResultCache();
            var cachedResult = new CallResultData(Maybe.Nothing<object>(), null);
            var call = Substitute.For<ICall>();

            var callSpec1 = Substitute.For<ICallSpecification>();
            callSpec1.IsSatisfiedBy(call).Returns(true);

            var callSpec2 = Substitute.For<ICallSpecification>();
            callSpec2.IsSatisfiedBy(call).Returns(true);

            // Act
            sut.AddResult(callSpec1, new CallResultData(Maybe.Nothing<object>(), null));
            sut.AddResult(callSpec2, cachedResult);

            CallResultData retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            // Assert
            Assert.True(hasResult);
            Assert.Same(cachedResult, retrievedResult);
        }

        [Fact]
        public void ShouldntReturnIfSpecificationIsNotSatisfied()
        {
            // Arrange
            var sut = new CallResultCache();

            var call = Substitute.For<ICall>();
            var callSpec = Substitute.For<ICallSpecification>();
            callSpec.IsSatisfiedBy(call).Returns(false);

            var cachedResult = new CallResultData(Maybe.Nothing<object>(), null);

            // Act
            sut.AddResult(callSpec, cachedResult);

            CallResultData retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            // Assert
            Assert.False(hasResult);
            Assert.Null(retrievedResult);
        }

        [Fact]
        public void ShouldCorrectlyHandleConcurrency()
        {
            // Arrange
            int threadsCount = 5;
            int itemsPerThread = 10;

            var dataRows = Enumerable.Range(0, threadsCount * itemsPerThread)
                .Select(_ =>
                {
                    var call = Substitute.For<ICall>();
                    var callSpec = Substitute.For<ICallSpecification>();
                    callSpec.IsSatisfiedBy(call).Returns(true);
                    var result = new CallResultData(Maybe.Nothing<object>(),
                        Enumerable.Empty<CallResultData.ArgumentValue>());

                    return (call: call, callSpec: callSpec, result: result);
                })
                .ToList();

            var sut = new CallResultCache();

            // Act
            dataRows
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(threadsCount)
                .Select(data =>
                {
                    sut.AddResult(data.callSpec, data.result);
                    return true;
                })
                .ToArray();

            // Assert
            Assert.True(dataRows.All(row =>
                sut.TryGetResult(row.call, out CallResultData result) && ReferenceEquals(result, row.result)));
        }

        [Fact]
        public void AddRowShouldFailForNullCallSpecification()
        {
            // Arrange
            var sut = new CallResultCache();
            var callResult = new CallResultData(Maybe.Nothing<object>(),
                Enumerable.Empty<CallResultData.ArgumentValue>());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.AddResult(null, callResult));
        }

        [Fact]
        public void AddRowShouldFailForNullCallResult()
        {
            // Arrange
            var sut = new CallResultCache();
            var callSpec = Substitute.For<ICallSpecification>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.AddResult(callSpec, null));
        }

        [Fact]
        public void TryGetValueShouldFailForNullCall()
        {
            // Arrange
            var sut = new CallResultCache();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.TryGetResult(null, out CallResultData _));
        }
    }
}