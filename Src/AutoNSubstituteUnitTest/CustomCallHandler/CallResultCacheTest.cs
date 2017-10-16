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
            // Fixture setup
            var sut = new CallResultCache();
            var cachedResult = new CallResultData(Maybe.Nothing<object>(), null);

            var call = Substitute.For<ICall>();
            var callSpec = Substitute.For<ICallSpecification>();
            callSpec.IsSatisfiedBy(call).Returns(true);

            // Exercise system
            sut.AddResult(callSpec, cachedResult);
            CallResultData retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            // Verify outcome
            Assert.True(hasResult);
            Assert.Same(cachedResult, retrievedResult);

            // Teardown
        }

        [Fact]
        public void TheLatestResultShouldBeReturned()
        {
            // Fixture setup
            var sut = new CallResultCache();
            var cachedResult = new CallResultData(Maybe.Nothing<object>(), null);
            var call = Substitute.For<ICall>();

            var callSpec1 = Substitute.For<ICallSpecification>();
            callSpec1.IsSatisfiedBy(call).Returns(true);

            var callSpec2 = Substitute.For<ICallSpecification>();
            callSpec2.IsSatisfiedBy(call).Returns(true);

            // Exercise system
            sut.AddResult(callSpec1, new CallResultData(Maybe.Nothing<object>(), null));
            sut.AddResult(callSpec2, cachedResult);

            CallResultData retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            // Verify outcome
            Assert.True(hasResult);
            Assert.Same(cachedResult, retrievedResult);

            // Teardown
        }

        [Fact]
        public void ShouldntReturnIfSpecificationIsNotSatisfied()
        {
            // Fixture setup
            var sut = new CallResultCache();

            var call = Substitute.For<ICall>();
            var callSpec = Substitute.For<ICallSpecification>();
            callSpec.IsSatisfiedBy(call).Returns(false);

            var cachedResult = new CallResultData(Maybe.Nothing<object>(), null);

            // Exercise system
            sut.AddResult(callSpec, cachedResult);

            CallResultData retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            // Verify outcome
            Assert.False(hasResult);
            Assert.Null(retrievedResult);

            // Teardown
        }

        [Fact]
        public void ShouldCorrectlyHandleConcurrency()
        {
            // Fixture setup
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

            // Exercise system
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

            // Verify outcome
            Assert.True(dataRows.All(row =>
                sut.TryGetResult(row.call, out CallResultData result) && ReferenceEquals(result, row.result)));

            // Teardown
        }

        [Fact]
        public void AddRowShouldFailForNullCallSpecification()
        {
            // Fixture setup
            var sut = new CallResultCache();
            var callResult = new CallResultData(Maybe.Nothing<object>(),
                Enumerable.Empty<CallResultData.ArgumentValue>());

            // Exercise system & Verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.AddResult(null, callResult));

            // Teardown
        }

        [Fact]
        public void AddRowShouldFailForNullCallResult()
        {
            // Fixture setup
            var sut = new CallResultCache();
            var callSpec = Substitute.For<ICallSpecification>();

            // Exercise system & Verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.AddResult(callSpec, null));

            // Teardown
        }

        [Fact]
        public void TryGetValueShouldFailForNullCall()
        {
            // Fixture setup
            var sut = new CallResultCache();

            // Exercise system & Verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.TryGetResult(null, out CallResultData _));

            // Teardown
        }
    }
}