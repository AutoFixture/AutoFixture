using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DelegatingRecursionHandler : IRecursionHandler
    {
        public DelegatingRecursionHandler()
        {
            this.OnHandleRecursiveRequest = (r, rs) => new object();
        }

        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            return this.OnHandleRecursiveRequest(request, recordedRequests);
        }

        internal Func<object, IEnumerable<object>, object> OnHandleRecursiveRequest { get; set; }
    }
}
