using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <inheritdoc />
    public class CallResultResolverFactory : ICallResultResolverFactory
    {
        /// <inheritdoc />
        public ICallResultResolver Create(ISpecimenContext specimenContext)
        {
            return new CallResultResolver(specimenContext);
        }
    }
}