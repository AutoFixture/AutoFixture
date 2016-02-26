using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal class ProxyType
    {
        private readonly ConstructorInfo constructor;
        private readonly IEnumerable<object> parameters;

        internal ProxyType(
            ConstructorInfo constructor,
            params object[] parameters)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            this.constructor = constructor;
            this.parameters = parameters;
        }

        internal ConstructorInfo Constructor
        {
            get { return this.constructor; }
        }

        internal IEnumerable<object> Parameters
        {
            get { return this.parameters; }
        }
    }
}
