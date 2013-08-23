using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.org.Addins.Builders
{
    public class AutoDataBuilder : ITestCaseBuilder2
    {
        public bool CanBuildFrom(MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public Test BuildFrom(MethodInfo method)
        {
            throw new NotImplementedException();
        }
        
        public bool CanBuildFrom(MethodInfo method, Test parentSuite)
        {
            throw new NotImplementedException();
        }

        public Test BuildFrom(MethodInfo method, Test parentSuite)
        {
            throw new NotImplementedException();
        }
    }
}