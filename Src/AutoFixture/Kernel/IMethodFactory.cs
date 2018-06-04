using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new Method from a <see cref="MethodInfo"/>.
    /// </summary>
    public interface IMethodFactory
    {
        /// <summary>
        /// Creates the method for the supplied methodInfo.
        /// </summary>
        /// <param name="methodInfo">The methodInfo.</param>
        /// <returns>Method for <paramref name="methodInfo"/>.</returns>
        IMethod Create(MethodInfo methodInfo);
    }
}
