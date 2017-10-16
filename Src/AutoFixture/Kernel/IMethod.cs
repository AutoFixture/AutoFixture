using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Represents some kind of method that can be invoked with a known set of parameters.
    /// </summary>
    public interface IMethod
    {
        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        IEnumerable<ParameterInfo> Parameters { get; }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        object Invoke(IEnumerable<object> parameters);
    }
}
