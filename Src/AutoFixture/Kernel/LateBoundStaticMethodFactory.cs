using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{    
    /// <summary>
    /// Creates a new <see cref="LateBoundMethod" /> for a static
    /// <see cref="MethodInfo"/>.
    /// </summary>
    public class LateBoundStaticMethodFactory : IMethodFactory
    {
        /// <summary>
        /// Creates a <see cref="StaticMethod" /> decorated with 
        /// <see cref="LateBoundMethod" /> for the supplied methodInfo.
        /// </summary>
        /// <param name="methodInfo">The methodInfo.</param>
        /// <returns>Method for <paramref name="methodInfo"/>.</returns>
        public IMethod Create(MethodInfo methodInfo)
        {
            return new LateBoundMethod(new StaticMethod(methodInfo));
        }
    }
}
