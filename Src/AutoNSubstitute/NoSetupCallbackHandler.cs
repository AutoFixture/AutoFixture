using System;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute
{
    [Obsolete("This class belongs to the legacy integration approach. " +
              "Use the NSubstituteRegisterCallHandlerCommand class and its dependencies instead.")]
    internal class NoSetupCallbackHandler : ICallHandler
    {
        private readonly ISubstituteState state;
        private readonly Action action;

        public NoSetupCallbackHandler(ISubstituteState state, Action action)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public bool HasResultFor(ICall call)
        {
            return this.state.CallResults.HasResultFor(call);
        }

        RouteAction ICallHandler.Handle(ICall call)
        {
            if (!this.HasResultFor(call))
                this.action();

            return RouteAction.Continue();
        }
    }
}