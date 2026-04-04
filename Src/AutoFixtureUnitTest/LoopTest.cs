using System;
using Xunit;

namespace AutoFixtureUnitTest;

internal class LoopTest<TSut, TResult>
    where TSut : new()
{
    private readonly Func<TSut, TResult> _create;

    internal LoopTest(Func<TSut, TResult> func)
    {
        _create = func;
    }

    public void Execute(int loopCount)
    {
        Execute(loopCount, (TResult)Convert.ChangeType(loopCount, typeof(TResult)));
    }

    public void Execute(int loopCount, TResult expectedResult)
    {
        // Arrange
        TSut sut = new TSut();
        // Act
        TResult result = default(TResult);
        for (int i = 0; i < loopCount; i++)
        {
            result = _create(sut);
        }
        // Assert
        Assert.Equal<TResult>(expectedResult, result);
    }
}