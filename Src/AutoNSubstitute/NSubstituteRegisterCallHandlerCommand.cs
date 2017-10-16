using System;
using AutoFixture.AutoNSubstitute.CustomCallHandler;
using AutoFixture.Kernel;
using NSubstitute.Core;
using NSubstitute.Exceptions;

namespace AutoFixture.AutoNSubstitute
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
    ///  - class's abstract/virtual/overridden/non-sealed methods/property getters;
    ///
    /// Notes:
    /// - Calling a method more than once with the same parameters will return the same value by default.
    ///   To override that behavior pass a custom <see cref="ICallResultCacheFactory"/> instance.
    /// - Methods inherited from <see cref="Object" /> are not set up due to a limitation in NSubstitute
    ///     (http://stackoverflow.com/a/21787891)
    /// </remarks>
    public class NSubstituteRegisterCallHandlerCommand : ISpecimenCommand
    {
        /// <summary>
        /// The NSubstitute context.
        /// </summary>
        public ISubstitutionContext SubstitutionContext { get; }

        /// <summary>
        /// Factory used to create <see cref="ICallResultCache"/> instances.
        /// </summary>
        public ICallResultCacheFactory CallResultCacheFactory { get; }

        /// <summary>
        /// Factory used to create <see cref="ICallResultResolver"/> instances.
        /// </summary>
        public ICallResultResolverFactory CallResultResolverFactory { get; }

        /// <summary>
        /// Creates an new <see cref="NSubstituteRegisterCallHandlerCommand"/> instance 
        /// with default <see cref="ICallResultCacheFactory"/> and <see cref="ICallResultResolverFactory"/> instanes.
        /// </summary>
        public NSubstituteRegisterCallHandlerCommand(ISubstitutionContext substitutionContext)
            : this(substitutionContext, new CallResultCacheFactory(), new CallResultResolverFactory())
        {
        }

        /// <summary>
        /// Creates an new <see cref="NSubstituteRegisterCallHandlerCommand"/> instance.
        /// </summary>
        public NSubstituteRegisterCallHandlerCommand(
            ISubstitutionContext substitutionContext,
            ICallResultCacheFactory callResultCacheFactory,
            ICallResultResolverFactory callResultResolverFactory)
        {
            if (substitutionContext == null) throw new ArgumentNullException(nameof(substitutionContext));
            if (callResultCacheFactory == null) throw new ArgumentNullException(nameof(callResultCacheFactory));
            if (callResultResolverFactory == null) throw new ArgumentNullException(nameof(callResultResolverFactory));

            this.SubstitutionContext = substitutionContext;
            this.CallResultCacheFactory = callResultCacheFactory;
            this.CallResultResolverFactory = callResultResolverFactory;
        }

        /// <summary>
        /// Registers a custom handler for the specified specimen if that is a substitute.
        /// Custom handler resolves call result and ref/out arguments using the passed <paramref name="context"/>.
        /// </summary>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException(nameof(specimen));
            if (context == null) throw new ArgumentNullException(nameof(context));

            ICallRouter router;

            try
            {
                router = this.SubstitutionContext.GetCallRouterFor(specimen);
            }
            catch (NotASubstituteException)
            {
                return;
            }

            // Add extensibility point for users to allow to use differnt cache implementation.
            // For intance users might want to disalbe results caching like that is demanded here:
            // https://github.com/AutoFixture/AutoFixture/issues/625
            var resultsCacheForSubstitution = this.CallResultCacheFactory.CreateCache();
            var callResultsResover = this.CallResultResolverFactory.Create(context);

            router.RegisterCustomCallHandlerFactory(
                substituteState =>
                    new AutoFixtureValuesHandler(
                        callResultsResover,
                        resultsCacheForSubstitution,
                        substituteState.CallSpecificationFactory));
        }
    }
}