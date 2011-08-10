using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    internal class ParameterScore : IComparable<ParameterScore>
    {
        private readonly int score;

        internal ParameterScore(Type parentType, Type targetType, IEnumerable<ParameterInfo> parameters)
        {
            if (parentType == null)
            {
                throw new ArgumentNullException("parentType");
            }
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            this.score = ParameterScore.CalculateScore(parentType, targetType, parameters);
        }

        public int CompareTo(ParameterScore other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.score.CompareTo(other.score);
        }

        private static int CalculateScore(Type parentType, Type targetType, IEnumerable<ParameterInfo> parameters)
        {
            var genericParameterTypes = parentType.GetGenericArguments();
            if (genericParameterTypes.Length != 1)
            {
                return 0;
            }
            var genericParameterType = genericParameterTypes.Single();

            var listType = targetType.MakeGenericType(genericParameterType);
            return parameters.Count(p => listType.IsAssignableFrom(p.ParameterType));
        }
    }
}
