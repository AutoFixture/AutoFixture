using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertyTypeAndNameCriterion : IEquatable<PropertyInfo>
    {
        private readonly IEquatable<Type> typeCriterion;
        private readonly IEquatable<string> nameCriterion;

        public PropertyTypeAndNameCriterion(
            IEquatable<Type> typeCriterion,
            IEquatable<string> nameCriterion)
        {
            this.typeCriterion = typeCriterion;
            this.nameCriterion = nameCriterion;
        }

        public bool Equals(PropertyInfo other)
        {
            if (other == null)
                return false;

            return this.typeCriterion.Equals(other.PropertyType)
                && this.nameCriterion.Equals(other.Name);
        }
    }
}