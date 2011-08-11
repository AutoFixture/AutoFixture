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
            var typeEqualityScore = parameters.Count(p => 
            {
                var gpt = p.ParameterType.GetGenericArguments();
                if (gpt.Length != 1)
                    return false;

                return p.ParameterType.GetGenericTypeDefinition() == targetType;
            });
            if (typeEqualityScore > 0)
                return typeEqualityScore;

            var genericParameterTypes = parentType.GetGenericArguments();
            if (genericParameterTypes.Length != 1)
            {
                return parameters.Count() * -1;
            }
            var genericParameterType = genericParameterTypes.Single();

            var listType = targetType.MakeGenericType(genericParameterType);

            var polymorphismScore = parameters.Count(p => listType.IsAssignableFrom(p.ParameterType));
            if (polymorphismScore <= 0)
            {
                return parameters.Count() * -1;
            }
            else
                return polymorphismScore;
        }
    }
}
