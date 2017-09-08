using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal class ProxyType
    {
        internal ProxyType(
            ConstructorInfo constructor,
            params object[] parameters)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            this.Constructor = constructor;
            this.Parameters = parameters;
        }

        internal ConstructorInfo Constructor { get; }

        internal IEnumerable<object> Parameters { get; }
    }
}
