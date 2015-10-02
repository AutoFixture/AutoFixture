using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Represents a criterion for comparing a candidate property against a
    /// desired type and name.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sometimes, you may need to evaluate various candidate
    /// <seealso cref="PropertyInfo" /> values, looking for one that has a
    /// desired property type and name. This class represents such an
    /// evaluation criterion.
    /// </para>
    /// </remarks>
    /// <seealso cref="Equals(PropertyInfo)" />
    /// <seealso cref="Criterion{T}" />
    /// <seealso cref="ParameterTypeAndNameCriterion" />
    /// <seealso cref="FieldTypeAndNameCriterion" />
    public class PropertyTypeAndNameCriterion : IEquatable<PropertyInfo>
    {
        private readonly IEquatable<Type> typeCriterion;
        private readonly IEquatable<string> nameCriterion;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PropertyTypeAndNameCriterion" /> class with the desired
        /// field type and name criteria.
        /// </summary>
        /// <param name="typeCriterion">
        /// The criterion indicating the desired field type.
        /// </param>
        /// <param name="nameCriterion">
        /// The criterion indicating the desired field name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeCriterion" /> or
        /// <paramref name="nameCriterion" /> is <see langword="null" />.
        /// </exception>
        public PropertyTypeAndNameCriterion(
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

        /// <summary>
        /// Compares a candidate <see cref="PropertyInfo" /> object against
        /// this <see cref="FieldTypeAndNameCriterion" />.
        /// </summary>
        /// <param name="other">
        /// The candidate to compare against this object.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="other" /> is deemed
        /// equal to this instance; otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method compares the candidate <paramref name="other" />
        /// against <see cref="TypeCriterion" /> and
        /// <see cref="NameCriterion" />. If the property's type matches the
        /// type criterion, and its name matches the name criterion, the return
        /// value is <see langword="true" />.
        /// </para>
        /// </remarks>
        public bool Equals(PropertyInfo other)
        {
            if (other == null)
                return false;

            return this.typeCriterion.Equals(other.PropertyType)
                && this.nameCriterion.Equals(other.Name);
        }

        /// <summary>
        /// Determines whether this object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="obj" /> is equal to this
        /// object; otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as PropertyTypeAndNameCriterion;
            if (other == null)
                return base.Equals(obj);

            return object.Equals(this.typeCriterion, other.typeCriterion)
                && object.Equals(this.nameCriterion, other.nameCriterion);
        }

        /// <summary>
        /// The type criterion originally passed in via the class' constructor.
        /// </summary>
        public IEquatable<Type> TypeCriterion
        {
            get { return this.typeCriterion; }
        }

        /// <summary>
        /// The name criterion originally passed in via the class' constructor.
        /// </summary>
        public IEquatable<string> NameCriterion
        {
            get { return this.nameCriterion; }
        }

        /// <summary>
        /// Returns the hash code for the object.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return 
                this.typeCriterion.GetHashCode() ^
                this.nameCriterion.GetHashCode();
        }
    }
}