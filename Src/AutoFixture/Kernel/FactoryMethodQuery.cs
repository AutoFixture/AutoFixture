using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Selects public factory methods ordered by the modest first.
    /// </summary>
    public class FactoryMethodQuery : IMethodQuery
    {
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
        /// <para>
        /// Factory methods that contain parameters of the requested <paramref name="type"/>
        /// are skipped in order to avoid circular references.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return from mi in type.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Public)
                   where mi.ReturnType == type &&
                         !string.Equals(mi.Name, "op_Implicit", StringComparison.Ordinal) &&
                         !string.Equals(mi.Name, "op_Explicit", StringComparison.Ordinal)
                   let parameters = mi.GetParameters()
                   where mi.GetParameters().All(p => p.ParameterType != type)
                   orderby parameters.Length ascending
                   select new StaticMethod(mi) as IMethod;
        }
    }
}
