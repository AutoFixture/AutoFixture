using System.Collections.Concurrent;
using System.Linq;
using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <inheritdoc />
    public class ResultsCache : IResultsCache
    {
        private ConcurrentQueue<ResultForCallSpec> CallResults { get; } = new ConcurrentQueue<ResultForCallSpec>();

        public void AddResult(ICallSpecification callSpecification, CallResultData result)
        {
            CallResults.Enqueue(new ResultForCallSpec(callSpecification, result));
        }

        public bool TryGetResult(ICall call, out CallResultData callResult)
        {
            callResult = null;

            var result = CallResults.Reverse().FirstOrDefault(c => c.IsResultFor(call));
            if (result == null) return false;

            callResult = result.Result;
            return true;
        }

        private class ResultForCallSpec
        {
            private readonly ICallSpecification _callSpecification;
            public CallResultData Result { get; }

            public ResultForCallSpec(ICallSpecification callSpecification, CallResultData result)
            {
                _callSpecification = callSpecification;
                Result = result;
            }

            public bool IsResultFor(ICall call) => _callSpecification.IsSatisfiedBy(call);
        }
    }
}