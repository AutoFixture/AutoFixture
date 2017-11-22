using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Selects public constructors ordered by the most greedy constructor first.
    /// </summary>
    public class GreedyConstructorQuery : IMethodQuery
    {
        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public constructors for <paramref name="type"/>, ordered by the most greedy
        /// constructor first.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned constructors is based on the number of parameters of the
        /// constructor. Constructors with more parameters are returned before constructors with
        /// fewer parameters. This means that if a default constructor exists, it will be the last
        /// one returned.
        /// </para>
        /// <para>
        /// In case of two constructors with an equal number of parameters, the ordering is
        /// unspecified.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return from ci in type.GetTypeInfo().GetConstructors()
                   let parameters = ci.GetParameters()
                   where parameters.All(p => p.ParameterType != type)
                   orderby parameters.Length descending
                   select new ConstructorMethod(ci) as IMethod;
        }
    }
}
