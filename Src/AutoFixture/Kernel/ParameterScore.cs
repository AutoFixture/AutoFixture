using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    internal class ParameterScore : IComparable<ParameterScore>
    {
        private readonly Type parentType;
        private readonly Type targetType;
        private readonly IEnumerable<ParameterInfo> parameters;

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

            this.parentType = parentType;
            this.targetType = targetType;
            this.parameters = parameters;
        }

        public int CompareTo(ParameterScore other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.CalculateScore().CompareTo(other.CalculateScore());
        }

        private int CalculateScore()
        {
            var genericParameterTypes = this.parentType.GetGenericArguments();
            if (genericParameterTypes.Length != 1)
            {
                return 0;
            }
            var genericParameterType = genericParameterTypes.Single();

            var listType = this.targetType.MakeGenericType(genericParameterType);
            return this.parameters.Count(p => listType.IsAssignableFrom(p.ParameterType));
        }
    }
}
