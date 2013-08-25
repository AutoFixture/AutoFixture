using System;
using NUnit.Core;

namespace Ploeh.AutoFixture.NUnit.Listeners
{
    public class AutoTestCaseEventListener : EventListener
    {
        public void RunStarted(string name, int testCount)
        {
            //throw new NotImplementedException();
        }

        public void RunFinished(TestResult result)
        {
            //throw new NotImplementedException();
        }

        public void RunFinished(Exception exception)
        {
            //throw new NotImplementedException();
        }

        public void TestStarted(TestName testName)
        {
            //throw new NotImplementedException();
        }

        public void TestFinished(TestResult result)
        {
            //throw new NotImplementedException();
        }

        public void SuiteStarted(TestName testName)
        {
            //throw new NotImplementedException();
        }

        public void SuiteFinished(TestResult result)
        {
            //throw new NotImplementedException();
        }

        public void UnhandledException(Exception exception)
        {
            //throw new NotImplementedException();
        }

        public void TestOutput(TestOutput testOutput)
        {
            //throw new NotImplementedException();
        }
    }
}