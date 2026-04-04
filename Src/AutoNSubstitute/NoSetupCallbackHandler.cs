using System;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute;

[Obsolete("This class belongs to the legacy integration approach. " +
          "Use the NSubstituteRegisterCallHandlerCommand class and its dependencies instead.")]
internal class NoSetupCallbackHandler : ICallHandler
{
    private readonly ISubstituteState _state;
    private readonly Action _action;

    public NoSetupCallbackHandler(ISubstituteState state, Action action)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public bool HasResultFor(ICall call)
    {
        return CompatShim.CallResults_HasCallResultFor(_state.CallResults, call);
    }

    RouteAction ICallHandler.Handle(ICall call)
    {
        if (!HasResultFor(call))
            _action();

        return RouteAction.Continue();
    }
}