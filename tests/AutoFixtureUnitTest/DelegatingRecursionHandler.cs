using System;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest;

public class DelegatingRecursionHandler : IRecursionHandler
{
    public DelegatingRecursionHandler()
    {
        OnHandleRecursiveRequest = (r, rs) => new object();
    }

    public object HandleRecursiveRequest(
        object request,
        IEnumerable<object> recordedRequests)
    {
        return OnHandleRecursiveRequest(request, recordedRequests);
    }

    internal Func<object, IEnumerable<object>, object> OnHandleRecursiveRequest { get; set; }
}