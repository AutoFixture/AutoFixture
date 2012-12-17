using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Selects appropriate constructors to create mock instances with Rhino Mocks.
    /// </summary>
#pragma warning disable 618
    public class NSubstituteConstructorQuery : IMethodQuery, IConstructorQuery
#pragma warning restore 618
    {
        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method returns a sequence of <see cref="NSubstituteConstructorMethod"/> according to
        /// the public and protected constructors available on <paramref name="type"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return SelectMethods(type);
        }

        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method returns a sequence of <see cref="NSubstituteConstructorMethod"/> according to
        /// the public and protected constructors available on <paramref name="type"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsInterface)
            {
                return new[] { new NSubstituteConstructorMethod(type, new ParameterInfo[0]) };
            }

            return from ci in type.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new NSubstituteConstructorMethod(ci.DeclaringType, paramInfos) as IMethod;
        }
    }
}
