using System;
using System.Collections.Concurrent;
using System.Linq;
using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <inheritdoc />
    public class CallResultCache : ICallResultCache
    {
        private ConcurrentStack<ResultForCallSpec> CallResults { get; } = new ConcurrentStack<ResultForCallSpec>();

        /// <inheritdoc />
        public void AddResult(ICallSpecification callSpecification, CallResultData result)
        {
            if (callSpecification == null) throw new ArgumentNullException(nameof(callSpecification));
            if (result == null) throw new ArgumentNullException(nameof(result));

            this.CallResults.Push(new ResultForCallSpec(callSpecification, result));
        }

        /// <inheritdoc />
        public bool TryGetResult(ICall callInfo, out CallResultData callResult)
        {
            if (callInfo == null) throw new ArgumentNullException(nameof(callInfo));

            var result = this.CallResults.FirstOrDefault(c => c.IsResultFor(callInfo));

            callResult = result?.Result;
            return result != null;
        }

        private class ResultForCallSpec
        {
            private readonly ICallSpecification callSpecification;
            public CallResultData Result { get; }

            public ResultForCallSpec(ICallSpecification callSpecification, CallResultData result)
            {
                this.callSpecification = callSpecification;
                this.Result = result;
            }

            public bool IsResultFor(ICall call) => this.callSpecification.IsSatisfiedBy(call);
        }
    }
}