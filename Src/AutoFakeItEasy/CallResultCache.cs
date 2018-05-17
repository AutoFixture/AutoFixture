using System;
using System.Collections.Concurrent;

namespace AutoFixture.AutoFakeItEasy
{
    internal class CallResultCache
    {
        private readonly ConcurrentDictionary<MethodCall, MethodCallResult> cachedResults =
            new ConcurrentDictionary<MethodCall, MethodCallResult>();

        public MethodCallResult GetOrAdd(MethodCall methodCall, Func<MethodCallResult> resultFactory)
        {
            return this.cachedResults.GetOrAdd(methodCall, key => resultFactory());
        }

        public void Put(MethodCall methodCall, MethodCallResult methodCallResult)
        {
            this.cachedResults[methodCall] = methodCallResult;
        }
    }
}
