﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ArgumentNullException(nameof(parentType));
            }
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
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
            var exactMatchScore = parameters.Count(p => ParameterScore.IsExactMatch(targetType, p));
            if (exactMatchScore > 0)
                return exactMatchScore;

            var genericParameterTypes = parentType.GetGenericArguments();
            if (genericParameterTypes.Length != 1)
                return parameters.Count() * -1;

            var genericParameterType = genericParameterTypes.Single();
            var listType = targetType.MakeGenericType(genericParameterType);

            var polymorphismScore = parameters.Count(p => listType == p.ParameterType);
            if (polymorphismScore > 0)
                return polymorphismScore;

            return parameters.Count() * -1;                
        }

        private static bool IsExactMatch(Type targetType, ParameterInfo p)
        {
            if (!p.ParameterType.IsGenericType)
                return false;

            var genericParameterTypes = p.ParameterType.GetGenericArguments();
            if (genericParameterTypes.Length != 1)
                return false;

            return p.ParameterType.GetGenericTypeDefinition() == targetType;
        }
    }
}
