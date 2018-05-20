using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new <see cref="MissingParametersSupplyingMethod" /> for a static
    /// <see cref="MethodInfo"/>.
    /// </summary>
    public class MissingParametersSupplyingStaticMethodFactory : IMethodFactory
    {
        /// <summary>
        /// Creates a <see cref="StaticMethod" /> decorated with
        /// <see cref="MissingParametersSupplyingMethod" /> for the supplied methodInfo.
        /// </summary>
        /// <param name="methodInfo">The methodInfo.</param>
        /// <returns>Method for <paramref name="methodInfo"/>.</returns>
        public IMethod Create(MethodInfo methodInfo)
        {
            return new MissingParametersSupplyingMethod(new StaticMethod(methodInfo));
        }
    }
}
