using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Selects public constructors ordered so that any constructor with arguments matching
    /// <see cref="IList{T}"/> are selected before any other public constructor.
    /// </summary>
    /// <remarks>
    /// The main target of this <see cref="IConstructorQuery" /> implementation is to pick
    /// <see cref="System.Collections.ObjectModel.Collection{T}(IList{T})" /> before any other
    /// constructor. This can be used to populate a Collection instance with a list of items.
    /// </remarks>
    public class ListFavoringConstructorQuery : IConstructorQuery
    {
        #region IConstructorQuery Members

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
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return from ci in type.GetConstructors()
                   let score = new ListScore(type, ci.GetParameters())
                   orderby score descending
                   select new ConstructorMethod(ci) as IMethod;
        }

        #endregion

#warning ListFavoringConstructorQuery+ListScore and EnumerableFavoringConstructorQuery+EnumerableScore are very similar.
        private class ListScore : IComparable<ListScore>
        {
            private readonly Type parentType;
            private readonly IEnumerable<ParameterInfo> parameters;

            public ListScore(Type parentType, IEnumerable<ParameterInfo> parameters)
            {
                if (parentType == null)
                {
                    throw new ArgumentNullException("parentType");
                }
                if (parameters == null)
                {
                    throw new ArgumentNullException("parameters");
                }

                this.parentType = parentType;
                this.parameters = parameters;
            }

            #region IComparable<ListScore> Members

            public int CompareTo(ListScore other)
            {
                return this.CalculateScore().CompareTo(other.CalculateScore());
            }

            #endregion

            private int CalculateScore()
            {
                var genericParameterTypes = this.parentType.GetGenericArguments();
                if (genericParameterTypes.Length != 1)
                {
                    return 0;
                }
                var genericParameterType = genericParameterTypes.Single();

                var listType = typeof(IList<>).MakeGenericType(genericParameterType);
                return this.parameters.Count(p => listType.IsAssignableFrom(p.ParameterType));
            }
        }
    }
}
