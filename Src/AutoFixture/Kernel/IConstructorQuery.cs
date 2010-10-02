using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Defines a strategy for selecting constructors from a type.
    /// </summary>
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
