using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Selects public constructors ordered so that any constructor with array arguments are
    /// selected before any other public constructor.
    /// </summary>
    /// <remarks>
    /// The main target of this <see cref="IMethodQuery" /> implementation is to pick constructors
    /// with array arguments before any other constructor.
    /// </remarks>
    public class ArrayFavoringConstructorQuery : IMethodQuery
    {
        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public constructors for <paramref name="type"/>, giving priority to any constructor
        /// with one or more array arguments.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Given several constructors, this implementation will favor those constructors which
        /// contain array arguments. Constructors with most matching arguments are returned before
        /// constructors with less matching arguments.
        /// </para>
        /// <para>
        /// Any other constructors are returned with the most modest constructors first.
        /// </para>
        /// </remarks>
        /// <seealso cref="ArrayFavoringConstructorQuery" />
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return from ci in type.GetTypeInfo().GetConstructors()
                   let score = new ArrayParameterScore(ci.GetParameters())
                   where ci.GetParameters().All(p => p.ParameterType != type)
                   orderby score descending
                   select new ConstructorMethod(ci) as IMethod;
        }

        private class ArrayParameterScore : IComparable<ArrayParameterScore>
        {
            private readonly int score;

            public ArrayParameterScore(IEnumerable<ParameterInfo> parameters)
            {
                if (parameters == null)
                    throw new ArgumentNullException(nameof(parameters));

                this.score = CalculateScore(parameters);
            }

            public int CompareTo(ArrayParameterScore other)
            {
                if (other == null)
                {
                    return 1;
                }

                return this.score.CompareTo(other.score);
            }

            private static int CalculateScore(IEnumerable<ParameterInfo> parameters)
            {
                var arrayScore = parameters.Count(p => p.ParameterType.IsArray);
                if (arrayScore > 0)
                    return arrayScore;

                return parameters.Count() * -1;
            }
        }
    }
}
