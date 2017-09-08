using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Defines a strategy for selecting constructors from a type.
    /// </summary>
    [Obsolete("Use IMethodQuery instead.", true)]
    public interface IConstructorQuery
    {
        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Constructors for <paramref name="type"/>.</returns>
        IEnumerable<IMethod> SelectConstructors(Type type);
    }
}
