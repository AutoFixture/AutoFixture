//http://youtrack.jetbrains.com/issue/RSRP-205480

using System;
using NUnit.Core;

namespace Ploeh.AutoFixture.NUnit.Addins.Reharper
{
    internal class EventListenerWrapper : EventListener
    {
        private readonly EventListener eventListener;

        public EventListenerWrapper(EventListener eventListener)
        {
            this.eventListener = eventListener;
        }

        public void RunStarted(string name, int testCount)
        {
            if (this.eventListener != null)
            {
                this.eventListener.RunStarted(name, testCount);
            }
        }

        public void RunFinished(TestResult result)
        {
            if (this.eventListener != null)
            {
                this.eventListener.RunFinished(result);
            }
        }

        public void RunFinished(Exception exception)
        {
            if (this.eventListener != null)
            {
                this.eventListener.RunFinished(exception);
            }
        }

        public void TestStarted(TestName testName)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestStarted(testName);
            }
        }

        public void TestFinished(TestResult result)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestFinished(result);
            }
        }

        public void SuiteStarted(TestName testName)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestStarted(testName);
            }
        }

        public void SuiteFinished(TestResult result)
        {
            if (this.eventListener != null)
            {
                this.eventListener.SuiteFinished(result);
            }
        }

        public void UnhandledException(Exception exception)
        {
            if (this.eventListener != null)
            {
                this.eventListener.UnhandledException(exception);
            }
        }

        public void TestOutput(TestOutput testOutput)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestOutput(testOutput);
            }
        }
    }
}