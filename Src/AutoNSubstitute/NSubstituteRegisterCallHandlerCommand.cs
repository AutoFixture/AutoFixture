using System;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Sets up a substitute object's methods so that the return values will be retrieved from a fixture,
    /// instead of being created directly by NSubstitute.
    ///
    /// This will setup any void/non-void virtual methods (including property getters and generics).
    /// </summary>
    /// <remarks>
    /// This will setup any void, non-void virtual methods.
    /// This includes:
    ///  - interface's methods/property getters;
    ///  - class's abstract/virtual/overridden/non-sealed methods/property getters.
    ///
    /// Notes:
    /// - Calling a method more than once with the same parameters will always return the same value
    /// - Methods inherited from <see cref="Object" /> are not set up due to a limitation in NSubstitute
    ///     (http://stackoverflow.com/a/21787891)
    /// </remarks>
    public class NSubstituteRegisterCallHandlerCommand : ISpecimenCommand
    {
        public ISubstitutionContext SubstitutionContext { get; }

        public NSubstituteRegisterCallHandlerCommand(ISubstitutionContext substitutionContext)
        {
            SubstitutionContext = substitutionContext;
        }

        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException(nameof(specimen));
            if (context == null) throw new ArgumentNullException(nameof(context));

            ICallRouter router;

            try
            {
                router = SubstitutionContext.GetCallRouterFor(specimen);
            }
            catch (NotASubstituteException)
            {
                return;
            }

            var resultsCacheForSubstitution = new ResultsCache();
            var callResultsResover = new CallResultResolver(context);

            router.RegisterCustomCallHandlerFactory(
                substituteState =>
                    new AutoFixtureValuesHandler(
                        callResultsResover,
                        resultsCacheForSubstitution,
                        substituteState.CallSpecificationFactory));
        }
    }
}