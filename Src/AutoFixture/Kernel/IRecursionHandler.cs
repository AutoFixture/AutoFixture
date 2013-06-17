using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface IRecursionHandler
    {
        object HandleRecursiveRequest(object request, IEnumerable<object> recordedRequests);
    }
}
