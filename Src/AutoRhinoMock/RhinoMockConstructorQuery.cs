using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    /// <summary>
    /// Selects appropriate constructors to create mock instances with Rhino Mocks.
    /// </summary>
    public class RhinoMockConstructorQuery : IConstructorQuery
    {
        #region IConstructorQuery Members

        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method returns a sequence of <see cref="RhinoMockConstructorMethod"/> according to
        /// the public and protected constructors available on <paramref name="type"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return from ci in type.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new RhinoMockConstructorMethod(ci.DeclaringType, paramInfos) as IMethod;
        }

        #endregion
    }
}
