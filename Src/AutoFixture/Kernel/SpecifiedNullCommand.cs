using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFixture.Kernel
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
    [Obsolete("This class is no longer used, and will be removed in future versions.", true)]
    public class SpecifiedNullCommand<T, TProperty> : ISpecifiedSpecimenCommand<T>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecifiedNullCommand{T, TProperty}"/> class with the supplied
        /// property picker expression.
        /// </summary>
        /// <param name="propertyPicker">An expression that identifies a property or field.</param>
        public SpecifiedNullCommand(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null) throw new ArgumentNullException(nameof(propertyPicker));

            this.Member = propertyPicker.GetWritableMember().Member;
        }

        /// <summary>
        /// Gets the member identified by the expression supplied through the constructor.
        /// </summary>
        public MemberInfo Member { get; }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="specimen">A specimen.</param>
        /// <param name="context">An <see cref="ISpecimenContext"/>.</param>
        public void Execute(T specimen, ISpecimenContext context)
        {
        }

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
            if (request == null) throw new ArgumentNullException(nameof(request));

            IEqualityComparer comparer = new MemberInfoEqualityComparer();
            return comparer.Equals(this.Member, request);
        }
    }
}
