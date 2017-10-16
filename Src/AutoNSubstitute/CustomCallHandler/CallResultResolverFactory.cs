using AutoFixture.Kernel;

namespace AutoFixture.AutoNSubstitute.CustomCallHandler
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