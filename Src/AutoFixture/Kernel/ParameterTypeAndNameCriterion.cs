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
            if (typeCriterion == null)
                throw new ArgumentNullException("typeCriterion");
            if (nameCriterion == null)
                throw new ArgumentNullException("nameCriterion");

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

        public IEquatable<Type> TypeCriterion
        {
            get { return this.typeCriterion; }
        }

        public IEquatable<string> NameCriterion
        {
            get { return this.nameCriterion; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as ParameterTypeAndNameCriterion;
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