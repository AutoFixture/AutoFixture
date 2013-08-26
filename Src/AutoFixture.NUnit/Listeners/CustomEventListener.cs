using System;
using NUnit.Core;

namespace Ploeh.AutoFixture.NUnit.Listeners
{
    public class CustomEventListener : EventListener
    {
        private readonly EventListener _eventListener;

        public CustomEventListener(EventListener eventListener)
        {
            _eventListener = eventListener;
        }

        public void RunStarted(string name, int testCount)
        {
            if (_eventListener != null)
                _eventListener.RunStarted(name, testCount);
        }

        public void RunFinished(TestResult result)
        {
            if (_eventListener != null)
                _eventListener.RunFinished(result);
        }

        public void RunFinished(Exception exception)
        {
            if (_eventListener != null)
                _eventListener.RunFinished(exception);
        }

        public void TestStarted(TestName testName)
        {
            if (_eventListener != null)
                _eventListener.TestStarted(testName);
        }

        public void TestFinished(TestResult result)
        {
            if (_eventListener != null)
                _eventListener.TestFinished(result);
        }

        public void SuiteStarted(TestName testName)
        {
            if (_eventListener != null)
                _eventListener.TestStarted(testName);
        }

        public void SuiteFinished(TestResult result)
        {
            if (_eventListener != null)
                _eventListener.SuiteFinished(result);
        }

        public void UnhandledException(Exception exception)
        {
            if (_eventListener != null)
                _eventListener.UnhandledException(exception);
        }

        public void TestOutput(TestOutput testOutput)
        {
            if (_eventListener != null)
                _eventListener.TestOutput(testOutput);
        }
    }
}