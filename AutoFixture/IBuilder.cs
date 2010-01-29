
namespace Ploeh.AutoFixture
{
    /// <summary>
    /// General-purpose interface to build objects.
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Creates a new object of whatever type the <see cref="IBuilder"/> instance builds.
        /// </summary>
        /// <returns>A new object.</returns>
        object Create(object seed);
    }
}
