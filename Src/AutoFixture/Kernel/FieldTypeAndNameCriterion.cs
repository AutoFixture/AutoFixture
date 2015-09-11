using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class FieldTypeAndNameCriterion : IEquatable<FieldInfo>
    {
        private readonly IEquatable<Type> typeCriterion;
        private readonly IEquatable<string> nameCriterion;

        public FieldTypeAndNameCriterion(
            IEquatable<Type> typeCriterion, 
            IEquatable<string> nameCriterion)
        {
            if (typeCriterion == null)
                throw new ArgumentNullException("typeCriterion");
            if (nameCriterion == null)
                throw new ArgumentNullException("nameCriterion");

            this.typeCriterion = typeCriterion;
            this.nameCriterion = nameCriterion;
        }

        public bool Equals(FieldInfo other)
        {
            if (other == null)
                return false;

            return this.typeCriterion.Equals(other.FieldType)
                && this.nameCriterion.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {

            var other = obj as FieldTypeAndNameCriterion;
            if (other == null)
                return base.Equals(obj);

            return object.Equals(this.typeCriterion, other.typeCriterion)
                && object.Equals(this.nameCriterion, other.nameCriterion);
        }

        public override int GetHashCode()
        {
            return 
                this.typeCriterion.GetHashCode() ^
                this.nameCriterion.GetHashCode();
        }
    }
}