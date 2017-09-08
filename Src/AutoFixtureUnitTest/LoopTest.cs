using System;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class LoopTest<TSut, TResult>
        where TSut : new()
    {
        private readonly Func<TSut, TResult> create;

        internal LoopTest(Func<TSut, TResult> func)
        {
            this.create = func;
        }

        public void Execute(int loopCount)
        {
            Execute(loopCount, (TResult)Convert.ChangeType(loopCount, typeof(TResult)));
        }

        public void Execute(int loopCount, TResult expectedResult)
        {
            // Fixture setup
            TSut sut = new TSut();
            // Exercise system
            TResult result = default(TResult);
            for (int i = 0; i < loopCount; i++)
            {
                result = this.create(sut);
            }
            // Verify outcome
            Assert.Equal<TResult>(expectedResult, result);
            // Teardown
        }
    }
}
