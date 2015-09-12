using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ParameterTypeAndNameCriterion : IEquatable<ParameterInfo>
    {
        private readonly IEquatable<Type> typeCriterion;
        private readonly IEquatable<string> nameCriterion;

        public ParameterTypeAndNameCriterion(
            IEquatable<Type> typeCriterion,
            IEquatable<string> nameCriterion)
        {
            this.typeCriterion = typeCriterion;
            this.nameCriterion = nameCriterion;
        }

        public bool Equals(ParameterInfo other)
        {
            if (other == null)
                return false;

            return this.typeCriterion.Equals(other.ParameterType)
                && this.nameCriterion.Equals(other.Name);
        }
    }
}