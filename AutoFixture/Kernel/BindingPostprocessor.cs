using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Performs post-processing for a specimen by assigning a value to a selected field or
    /// property.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    /// <typeparam name="TProperty">The type of the property or field.</typeparam>
    public class BindingPostprocessor<T, TProperty> : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingPostprocessor{T, TProperty}"/> type
        /// with an expression that identifies a property or field.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="propertyPicker">An expression that identifies a property or field.</param>
        /// <remarks>
        /// <para>
        /// This constructor implies that an anonymous value will be assigned to the property or
        /// field, since no value is suppled.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This follows the same coding idiom as LINQ to SQL and LINQ to Entities. Since Funcs have implicit conversions into Expressions, usage is not as bad as it could have been. In any case, the desired functionality cannot be implemented in any other way while preserving static type checking.")]
        public BindingPostprocessor(ISpecimenBuilder builder, Expression<Func<T, TProperty>> propertyPicker)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            this.builder = builder;

            var me = propertyPicker.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("The expression's Body is not a MemberExpression. Most likely this is because it does not represent access to a property or field.", "propertyPicker");
            }
            var pi = me.Member as PropertyInfo;
            if (pi != null && pi.GetSetMethod() == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The property \"{0}\" is read-only.", pi.Name), "propertyPicker");
            }
            this.member = me.Member;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request and assigns a value to a field or property on
        /// the specimen before returning it.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created by the decorated <see cref="ISpecimenBuilder"/>, eriched by having
        /// the correct property or field assigned a value.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            var specimen = this.builder.Create(request, container);
            if (!(specimen is T))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen returned by the decorated ISpecimenBuilder is not compatible with {0}.", typeof(T)));
            }

            var bindingValue = container.Create(this.member);
            if (!(bindingValue is TProperty))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen created for assignment is not compatible with {0}.", typeof(TProperty)));
            }
            this.SetValue(specimen, bindingValue);

            return specimen;
        }

        #endregion

        private void SetValue(object specimen, object bindingValue)
        {
            var pi = this.member as PropertyInfo;
            if (pi != null)
            {
                pi.SetValue(specimen, bindingValue, null);
                return;
            }

            var fi = this.member as FieldInfo;
            if (fi != null)
            {
                fi.SetValue(specimen, bindingValue);
            }
        }
    }
}
