using System;
using System.Collections.Concurrent;

namespace AutoFixture.AutoFakeItEasy;

internal class CallResultCache
{
    private readonly ConcurrentDictionary<MethodCall, MethodCallResult> _cachedResults =
        new ConcurrentDictionary<MethodCall, MethodCallResult>();

    public MethodCallResult GetOrAdd(MethodCall methodCall, Func<MethodCallResult> resultFactory)
    {
        return _cachedResults.GetOrAdd(methodCall, key => resultFactory());
    }

    public void Put(MethodCall methodCall, MethodCallResult methodCallResult)
    {
        _cachedResults[methodCall] = methodCallResult;
    }
}