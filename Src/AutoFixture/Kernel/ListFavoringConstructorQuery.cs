using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ListFavoringConstructorQuery : IConstructorQuery
    {
        #region IConstructorQuery Members

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
