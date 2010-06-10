using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A no-op that identifies a certain property or field.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    /// <typeparam name="TProperty">The type of property or field.</typeparam>
    /// <remarks>
    /// <para>
    /// This class can be used to reserve an identified property or field without doing anything
    /// with it.
    /// </para>
    /// </remarks>
    public class SpecifiedNullCommand<T, TProperty> : ISpecifiedSpecimenCommand<T>
    {
        private readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecifiedNullCommand{T, TProperty}"/> class with the supplied
        /// property picker expression.
        /// </summary>
        /// <param name="propertyPicker">An expression that identifies a property or field.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This follows the same coding idiom as LINQ to SQL and LINQ to Entities. Since Funcs have implicit conversions into Expressions, usage is not as bad as it could have been. In any case, the desired functionality cannot be implemented in any other way while preserving static type checking.")]
        public SpecifiedNullCommand(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            this.member = propertyPicker.GetWritableMember().Member;
        }

        public MemberInfo Member
        {
            get { return this.member; }
        }

        #region ISpecifiedSpecimenCommand<T> Members

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="specimen">A specimen.</param>
        /// <param name="container">An <see cref="ISpecimenContainer"/>.</param>
        public void Execute(T specimen, ISpecimenContainer container)
        {
        }

        #endregion

        #region IRequestSpecification Members

        /// <summary>
        /// Evaluates whether a request matches the property or field reserved by this command.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is is a <see cref="PropertyInfo"/>
        /// or <see cref="FieldInfo"/> that identifies the property or field reserved by this
        /// <see cref="BindingCommand{T, TProperty}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return this.member.Equals(request);
        }

        #endregion
    }
}
