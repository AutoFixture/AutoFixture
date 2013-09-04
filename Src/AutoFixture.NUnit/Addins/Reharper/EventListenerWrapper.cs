using System;
using NUnit.Core;

namespace Ploeh.AutoFixture.NUnit.Addins.Reharper
{
    /// <summary>
    /// The EventListenerWrapper is used wrap any existing eventlistner and relay the events.
    /// </summary>
    /// <remarks>
    /// This is needed because of a resharper bug where it doesn't pick up NUnitAddin's
    /// http://youtrack.jetbrains.com/issue/RSRP-205480
    /// </remarks>
    internal class EventListenerWrapper : EventListener
    {
        private readonly EventListener eventListener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventListener"></param>
        public EventListenerWrapper(EventListener eventListener)
        {
            this.eventListener = eventListener;
        }

        /// <summary>
        /// Called when a test run is starting
        /// </summary>
        /// <param name="name">The name of the test being started</param>
        /// <param name="testCount">The number of test cases under this test</param>
        public void RunStarted(string name, int testCount)
        {
            if (this.eventListener != null)
            {
                this.eventListener.RunStarted(name, testCount);
            }
        }

        /// <summary>
        /// Called when a run finishes normally
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void RunFinished(TestResult result)
        {
            if (this.eventListener != null)
            {
                this.eventListener.RunFinished(result);
            }
        }

        /// <summary>
        /// Called when a run is terminated due to an exception
        /// </summary>
        /// <param name="exception">Exception that was thrown</param>
        public void RunFinished(Exception exception)
        {
            if (this.eventListener != null)
            {
                this.eventListener.RunFinished(exception);
            }
        }

        /// <summary>
        /// Called when a test case is starting
        /// </summary>
        /// <param name="testName">The name of the test case</param>
        public void TestStarted(TestName testName)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestStarted(testName);
            }
        }

        /// <summary>
        /// Called when a test case has finished
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void TestFinished(TestResult result)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestFinished(result);
            }
        }

        /// <summary>
        /// Called when a suite is starting
        /// </summary>
        /// <param name="testName">The name of the suite</param>
        public void SuiteStarted(TestName testName)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestStarted(testName);
            }
        }

        /// <summary>
        /// Called when a suite has finished
        /// </summary>
        /// <param name="result">The result of the suite</param>
        public void SuiteFinished(TestResult result)
        {
            if (this.eventListener != null)
            {
                this.eventListener.SuiteFinished(result);
            }
        }

        /// <summary>
        /// Called when an unhandled exception is detected during
        /// the execution of a test run.
        /// </summary>
        /// <param name="exception">The exception thta was detected</param>
        public void UnhandledException(Exception exception)
        {
            if (this.eventListener != null)
            {
                this.eventListener.UnhandledException(exception);
            }
        }

        /// <summary>
        /// Called when the test direts output to the console.
        /// </summary>
        /// <param name="testOutput">A console message</param>
        public void TestOutput(TestOutput testOutput)
        {
            if (this.eventListener != null)
            {
                this.eventListener.TestOutput(testOutput);
            }
        }
    }
}