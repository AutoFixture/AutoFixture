using System;
using System.Collections;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Provides custom equality comparison for requests of a property and field.
    /// </summary>
    public class MemberComparer : IMemberComparer
    {
        private readonly IEqualityComparer comparer;
        private readonly ISpecification<PropertyInfo> propertySpecification;
        private readonly ISpecification<FieldInfo> fieldSpecification;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberComparer"/> 
        /// class with the supplied <see cref="IEqualityComparer"/> to support
        /// the comparison of properties and fields.
        /// </summary>
        /// <param name="comparer">
        /// The supplied <see cref="IEqualityComparer"/>.
        /// </param>
        public MemberComparer(IEqualityComparer comparer)
            : this(
                comparer,
                new TrueSpecification<PropertyInfo>(),
                new TrueSpecification<FieldInfo>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberComparer" />
        /// class with the supplied <see cref="IEqualityComparer" /> to support
        /// the comparison of properties and fields.
        /// </summary>
        /// <param name="comparer">
        /// The supplied <see cref="IEqualityComparer" />.
        /// </param>
        /// <param name="propertySpecification">
        /// The supplied <see cref="ISpecification&lt;PropertyInfo&gt;" />.
        /// </param>
        /// <param name="fieldSpecification">
        /// The supplied <see cref="ISpecification&lt;FieldInfo&gt;" />.
        /// </param>
        public MemberComparer(
            IEqualityComparer comparer,
            ISpecification<PropertyInfo> propertySpecification,
            ISpecification<FieldInfo> fieldSpecification)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            if (propertySpecification == null)
                throw new ArgumentNullException("propertySpecification");

            if (fieldSpecification == null)
                throw new ArgumentNullException("fieldSpecification");

            this.comparer = comparer;
            this.propertySpecification = propertySpecification;
            this.fieldSpecification = fieldSpecification;
        }

        /// <summary>
        /// Gets the supplied <see cref="IEqualityComparer"/>.
        /// </summary>
        /// <value>
        /// The supplied <see cref="IEqualityComparer"/>.
        /// </value>
        public IEqualityComparer Comparer
        {
            get { return this.comparer; }
        }

        public ISpecification<PropertyInfo> PropertySpecification
        {
            get { return this.propertySpecification; }
        }

        public ISpecification<FieldInfo> FieldSpecification
        {
            get { return this.fieldSpecification; }
        }

        /// <summary>
        /// Evaluates a request for comparison of a property.
        /// </summary>
        /// <param name="request">The request for comparison of a property.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsSatisfiedBy(PropertyInfo request)
        {
            return this.propertySpecification.IsSatisfiedBy(request);
        }

        /// <summary>
        /// Evaluates a request for comparison of a field.
        /// </summary>
        /// <param name="request">The request for comparison of a field.</param>
        /// <returns><see langword="true"/>.</returns>
        public bool IsSatisfiedBy(FieldInfo request)
        {
            return this.fieldSpecification.IsSatisfiedBy(request);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is 
        /// equal to this instance.
        /// </summary>
        /// <param name="x">The <see cref="System.Object" /> to compare with
        /// this instance.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> 
        ///   is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///   <para>
        ///     <paramref name="x"/> and <paramref name="y"/> are considered
        ///     equal if the supplied <see cref="IEqualityComparer"/>'s Equals 
        ///     method returns <see langword="true"/>.
        ///   </para>
        /// </remarks>
        public new bool Equals(object x, object y)
        {
            return this.comparer.Equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(object obj)
        {
            return this.comparer.GetHashCode(obj);
        }
    }
}
