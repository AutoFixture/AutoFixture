using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    /// <summary>
    /// Selects appropriate constructors to create mock instances with Rhino Mocks.
    /// </summary>
#pragma warning disable 618
    public class RhinoMockConstructorQuery : IMethodQuery, IConstructorQuery
#pragma warning restore 618
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
            return this.SelectMethods(type);
        }

        #endregion

        #region IMethodQuery Members

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsInterface)
            {
                return new[] { new RhinoMockConstructorMethod(type, new ParameterInfo[0]) };
            }

            return from ci in type.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new RhinoMockConstructorMethod(ci.DeclaringType, paramInfos) as IMethod;
        }

        #endregion
    }
}
