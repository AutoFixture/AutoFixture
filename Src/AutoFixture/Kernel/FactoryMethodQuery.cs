using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Selects public factory methods ordered by the modest first.
    /// </summary>
    public class FactoryMethodQuery : IConstructorQuery
    {
        #region IConstructorPicker Members

        /// <summary>
        /// Selects the public factory methods for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public factory methods for <paramref name="type"/>, ordered by the modest first.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned methods is based on the number of parameters of the
        /// method. Methods with fewer parameters are returned before methods with more
        /// parameters. This means that if a default parameterless factory methods exists, it
        /// will be the first one returned.
        /// </para>
        /// <para>
        /// In case of two factory methods with an equal number of parameters, the ordering is
        /// unspecified.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return from mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                   where mi.ReturnType == type
                   let parameters = mi.GetParameters()
                   orderby parameters.Length ascending
                   select new FactoryMethod(mi) as IMethod;
        }

        #endregion
    }
}
