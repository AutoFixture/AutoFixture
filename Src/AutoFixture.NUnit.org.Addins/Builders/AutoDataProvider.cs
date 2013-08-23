using System;
using System.Collections;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.org.Addins.Builders
{
    public class AutoDataProvider : ITestCaseProvider2
    {
        public bool HasTestCasesFor(MethodInfo method)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable GetTestCasesFor(MethodInfo method)
        {
            throw new NotImplementedException();
        }
        
        public bool HasTestCasesFor(MethodInfo method, Test parentSuite)
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetTestCasesFor(MethodInfo method, Test parent)
        {
            throw new NotImplementedException();
        }
    }
}