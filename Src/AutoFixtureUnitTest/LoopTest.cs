using System;
using Xunit;

namespace AutoFixtureUnitTest
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
            this.Execute(loopCount, (TResult)Convert.ChangeType(loopCount, typeof(TResult)));
        }

        public void Execute(int loopCount, TResult expectedResult)
        {
            // Arrange
            TSut sut = new TSut();
            // Act
            TResult result = default(TResult);
            for (int i = 0; i < loopCount; i++)
            {
                result = this.create(sut);
            }
            // Assert
            Assert.Equal<TResult>(expectedResult, result);
        }
    }
}
