using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2
{
    internal class InlineAutoDataDiscoverer : DataDiscoverer
    {
        public override bool SupportsDiscoveryEnumeration(
            IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            // The data return by AutoDataAttribute is (like AutoFixture itself) typically
            // not 'stable'. In other words, the data often changes (e.g. string guids), and
            // therefore pre-discovery of tests decorated with this attribute is not possible.
            return false;
        }
    }
}
