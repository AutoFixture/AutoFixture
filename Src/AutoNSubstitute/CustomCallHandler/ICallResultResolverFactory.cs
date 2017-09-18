using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <summary>
    /// Factory to create <see cref="ICallResultResolver"/>.
    /// </summary>
    public interface ICallResultResolverFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ICallResultResolver"/>.
        /// </summary>
        /// <param name="specimenContext">The <see cref="ISpecimenContext"/> to use to create speciments.</param>
        /// <returns>The call result resolver.</returns>
        ICallResultResolver Create(ISpecimenContext specimenContext);
    }
}