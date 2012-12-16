using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>Selects appropriate methods to create substitutes.</summary>
    public class NSubstituteMethodQuery : IMethodQuery
    {
        /// <summary>Selects the methods for the supplied type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>Methods for <paramref name="type"/>.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type.IsInterface)
                return new[] { type.GetSubstituteFactoryMethod() };

            return from ci in type.GetPublicAndProtectedConstructors()
                   let parameters = ci.GetParameters()
                   orderby parameters.Length ascending
                   select type.GetSubstituteFactoryMethod(parameters);
        }
    }
}
