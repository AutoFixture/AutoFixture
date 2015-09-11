using System;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FieldTypeAndNameCriterion : IEquatable<FieldInfo>
    {
        private readonly IEquatable<Type> typeCriterion;
        private readonly IEquatable<string> nameCriterion;

        public FieldTypeAndNameCriterion(
            IEquatable<Type> typeCriterion, 
            IEquatable<string> nameCriterion)
        {
            this.typeCriterion = typeCriterion;
            this.nameCriterion = nameCriterion;
        }

        public bool Equals(FieldInfo other)
        {
            return this.typeCriterion.Equals(other.FieldType)
                && this.nameCriterion.Equals(other.Name);
        }
    }
}