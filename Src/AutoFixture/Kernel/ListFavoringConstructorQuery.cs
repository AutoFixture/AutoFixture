using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Selects public constructors ordered so that any constructor with arguments matching
    /// <see cref="IList{T}"/> are selected before any other public constructor.
    /// </summary>
    /// <remarks>
    /// The main target of this <see cref="IMethodQuery" /> implementation is to pick
    /// <see cref="System.Collections.ObjectModel.Collection{T}(IList{T})" /> before any other
    /// constructor. This can be used to populate a Collection instance with a list of items.
    /// </remarks>
    public class ListFavoringConstructorQuery : IMethodQuery
    {
        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public constructors for <paramref name="type"/>, giving priority to any constructor
        /// with one or more <see cref="IList{T}"/> arguments.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Given several constructors, this implementation will favor those constructors with
        /// arguments that matches <see cref="IList{T}"/>, where T is the item type of
        /// <paramref name="type"/>, if it's generic. Constructors with most matching arguments are
        /// returned before constructors with less matching arguments.
        /// </para>
        /// <para>
        /// Any other constructors are returned with the most modest constructors first.
        /// </para>
        /// </remarks>
        /// <seealso cref="ListFavoringConstructorQuery" />
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return from ci in type.GetTypeInfo().GetConstructors()
                   let score = new ParameterScore(type, typeof(IList<>), ci.GetParameters())
                   where ci.GetParameters().All(p => p.ParameterType != type)
                   orderby score descending
                   select new ConstructorMethod(ci) as IMethod;
        }
    }
}
