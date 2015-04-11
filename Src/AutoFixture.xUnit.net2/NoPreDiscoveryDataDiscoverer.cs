using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2
{
    /// <summary>
    /// Prevents Xunit 'pre-discovery' of tests that use the
    /// <see cref="AutoDataAttribute"/> or <see cref="InlineAutoDataAttribute"/>.
    /// </summary>
    public class NoPreDiscoveryDataDiscoverer : DataDiscoverer
    {
        /// <summary>
        /// Always returns 'false', indicating that discovery of tests is
        /// not supported.
        /// </summary>
        /// <param name="dataAttribute">The attribute</param>
        /// <param name="testMethod">The method being discovered</param>
        /// <returns>false</returns>
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
