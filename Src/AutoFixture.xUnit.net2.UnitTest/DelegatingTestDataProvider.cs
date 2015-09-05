using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    internal class DelegatingTestDataProvider : ITestDataProvider
    {
        public Func<MethodInfo, IEnumerable<object>> OnGetData = method => Enumerable.Empty<object>();

        public IEnumerable<object> GetData(MethodInfo testMethod)
        {
            return this.OnGetData(testMethod);
        }
    }
}
