using System;
using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Defines a strategy for selecting methods (such as constructors or factory methods) from a
    /// type.
    /// </summary>
    public interface IMethodQuery
    {
        /// <summary>
        /// Selects the methods for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        IEnumerable<IMethod> SelectMethods(Type type);
    }
}
