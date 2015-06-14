using System;
using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal class NoSetupCallbackHandler : ICallHandler
    {
        private readonly ISubstituteState state;
        private readonly Action action;

        public NoSetupCallbackHandler(ISubstituteState state, Action action)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            if (action == null)
                throw new ArgumentNullException("action");

            this.state = state;
            this.action = action;
        }

        public bool HasResultFor(ICall call)
        {
            return this.state.CallResults.HasResultFor(call);
        }

        RouteAction ICallHandler.Handle(ICall call)
        {
            if (!HasResultFor(call))
                action();

            return RouteAction.Continue();
        }
    }
}