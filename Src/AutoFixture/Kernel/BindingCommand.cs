using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a command that binds a property or a field to a value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the specimen on which the property or value will be set.
    /// </typeparam>
    /// <typeparam name="TProperty">The type of property or field.</typeparam>
#pragma warning disable 618
    public class BindingCommand<T, TProperty> : ISpecimenCommand, ObsoletedMemberShims.ISpecifiedSpecimenCommand<T>
#pragma warning restore 618
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingCommand{T, TProperty}"/> class with
        /// the supplied property picker expression.
        /// </summary>
        /// <param name="propertyPicker">An expression that identifies a property or field.</param>
        /// <remarks>
        /// <para>
        /// This constructor implies that an anonymous value will be assigned to the property or
        /// field identified by <paramref name="propertyPicker"/>.
        /// </para>
        /// </remarks>
        public BindingCommand(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null) throw new ArgumentNullException(nameof(propertyPicker));

            this.Member = propertyPicker.GetWritableMember().Member;
            this.ValueCreator = this.CreateAnonymousValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingCommand{T, TProperty}"/> class with
        /// the supplied property picker expression and the value to be assigned to that property
        /// or field.
        /// </summary>
        /// <param name="propertyPicker">An expression that identifies a property or field.</param>
        /// <param name="propertyValue">
        /// The value to assign to the property or field identified by
        /// <paramref name="propertyPicker"/>.
        /// </param>
        public BindingCommand(Expression<Func<T, TProperty>> propertyPicker, TProperty propertyValue)
        {
            if (propertyPicker == null) throw new ArgumentNullException(nameof(propertyPicker));

            this.Member = propertyPicker.GetWritableMember().Member;
            this.ValueCreator = c => propertyValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingCommand{T, TProperty}"/> class with
        /// the supplied property picker expression and a function that creates a value to be
        /// assigned to that property or field.
        /// </summary>
        /// <param name="propertyPicker">An expression that identifies a property or field.</param>
        /// <param name="valueCreator">
        /// A function that creates a value that will be assigned to the property or field
        /// identified by <paramref name="propertyPicker"/>.
        /// </param>
        public BindingCommand(Expression<Func<T, TProperty>> propertyPicker, Func<ISpecimenContext, TProperty> valueCreator)
        {
            if (propertyPicker == null) throw new ArgumentNullException(nameof(propertyPicker));
            if (valueCreator == null) throw new ArgumentNullException(nameof(valueCreator));

            this.Member = propertyPicker.GetWritableMember().Member;
            this.ValueCreator = valueCreator;
        }

        /// <summary>
        /// Gets the member identified by the expression supplied through the constructor.
        /// </summary>
        public MemberInfo Member { get; }

        /// <summary>
        /// Gets the function that creates a value to be assigned to the property or field
        /// identified by <see cref="Member"/>.
        /// </summary>
        public Func<ISpecimenContext, TProperty> ValueCreator { get; }

        /// <summary>
        /// Executes the command on the supplied specimen by assigning the property of field the
        /// correct value.
        /// </summary>
        /// <param name="specimen">
        /// A specimen that should have its property or field assigned.
        /// </param>
        /// <param name="context">
        /// An <see cref="ISpecimenContext"/> which can supply an anonymous value for the
        /// property or field.
        /// </param>
        /// <remarks>
        /// <para>
        /// This method assigns a value to the property or field identified by the expression
        /// supplied to the class' constructor. If no value (or creator) was supplied to the
        /// constructor, <paramref name="context"/> will be used to create the value.
        /// </para>
        /// </remarks>
        [Obsolete("This method is no longer used and will be removed in future versions. Please use the Execute(object, ISpecimenContext) overload instead.")]
        public void Execute(T specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException(nameof(specimen));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var bindingValue = this.ValueCreator(context);

            var pi = this.Member as PropertyInfo;
            if (pi != null)
            {
                pi.SetValue(specimen, bindingValue, null);
            }

            var fi = this.Member as FieldInfo;
            if (fi != null)
            {
                fi.SetValue(specimen, bindingValue);
            }
        }

        /// <summary>
        /// Evaluates whether a request matches the property or field affected by this command.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="PropertyInfo"/>
        /// or <see cref="FieldInfo"/> that identifies the property or field affected by this
        /// <see cref="BindingCommand{T, TProperty}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [Obsolete("This method is no longer used and will be removed in future versions. Please use this.Member property for specification instead.")]
        public bool IsSatisfiedBy(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            IEqualityComparer comparer = new MemberInfoEqualityComparer();
            return comparer.Equals(this.Member, request);
        }

        private TProperty CreateAnonymousValue(ISpecimenContext container)
        {
            var bindingValue = container.Resolve(this.Member);
            if ((bindingValue != null) && !(bindingValue is TProperty))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen created for assignment is not compatible with {0}.", typeof(TProperty)));
            }
            return (TProperty)bindingValue;
        }

        /// <summary>
        /// Executes the command on the supplied specimen by assigning the property of field the correct value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assigns a value to the property or field identified by
        /// the expression supplied to the class' constructor. If no value (or
        /// creator) was supplied to the constructor,
        /// <paramref name="context" /> will be used to create the value.
        /// </para>
        /// </remarks>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException(nameof(specimen));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var bindingValue = this.ValueCreator(context);

            var pi = this.Member as PropertyInfo;
            if (pi != null)
            {
                TrySetValue(
                    specimen,
                    bindingValue,
                    pi.PropertyType,
                    (s, v) => pi.SetValue(s, v, null));
            }

            var fi = this.Member as FieldInfo;
            if (fi != null)
            {
                TrySetValue(
                    specimen,
                    bindingValue,
                    fi.FieldType,
                    (s, v) => fi.SetValue(s, v));
            }
        }

        private static void TrySetValue(
            object specimen,
            object value,
            Type targetType,
            Action<object, object> setValue)
        {
            try
            {
                setValue(specimen, value);
            }
            catch (ArgumentException)
            {
                if (!(value is IConvertible))
                    throw;

                setValue(
                    specimen,
                    Convert.ChangeType(
                        value,
                        targetType,
                        CultureInfo.CurrentCulture));
            }
        }
    }
}
