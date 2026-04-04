using System;
using AutoFixture.Idioms;

namespace AutoFixture.IdiomsUnitTest;

public class DelegatingBehaviorExpectation : IBehaviorExpectation
{
    public DelegatingBehaviorExpectation()
    {
        OnVerify = c => { };
    }

    public Action<IGuardClauseCommand> OnVerify { get; set; }

    public void Verify(IGuardClauseCommand command)
    {
        OnVerify(command);
    }
}