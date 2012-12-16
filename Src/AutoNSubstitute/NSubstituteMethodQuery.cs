using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class NSubstituteMethodQuery : IMethodQuery
    {
        /// <summary>Selects the methods for the supplied type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
