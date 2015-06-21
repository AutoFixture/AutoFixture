using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Serves as a base class for specimen builders that relay requests made for properties, fields or parameters to 
    /// different requests, based on custom attributes applied to the code elements. 
    /// </summary>
    /// <remarks>
    /// A typical example is relaying a request for a parameter that has <see cref="RangeAttribute"/> applied to it 
    /// to a <see cref="RangedNumberRequest"/>. Another way to describe it, a request for a <see cref="ParameterInfo"/>
    /// instance that contains a <see cref="RangeAttribute"/>, is transformed to a <see cref="RangedNumberRequest"/>
    /// with parameters extracted from the attribute.
    /// </remarks>
    /// <typeparam name="TAttribute">
    /// An <see cref="Attribute"/> type that can be applied to a property, a field or a method. The attribute is 
    /// expected to allow applying only once. When the same attribute is applied multiple times to the same code 
    /// element, an <see cref="InvalidOperationException"/> will be thrown.
    /// </typeparam>
    public abstract class AttributeRelay<TAttribute> : ISpecimenBuilder
        where TAttribute : Attribute
    {
        /// <summary>
        /// Creates a relayed request based on a custom attribute applied to a code element and resolves it from the 
        /// given <paramref name="context"/>.
        /// </summary>
        /// <returns>
        /// A specimen resolved from the <paramref name="context"/> based on a relayed request.
        /// If the <paramref name="request"/> code element does not have attribute of expected type applied to it, 
        /// returns <see cref="NoSpecimen"/>.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var customAttributeProvider = request as ICustomAttributeProvider;
            if (customAttributeProvider == null)
            {
                return new NoSpecimen(request);
            }

            TAttribute attribute;

            try
            {
                attribute = customAttributeProvider.GetCustomAttributes(typeof(TAttribute), true)
                    .OfType<TAttribute>().SingleOrDefault();
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture, 
                        "Multiple instances of {0} were found for {1}.", 
                        typeof(TAttribute), request), 
                    e);
            }

            if (attribute == null)
            {
                return new NoSpecimen(request);
            }

            object relayedRequest = this.CreateRelayedRequest(customAttributeProvider, attribute);
            return context.Resolve(relayedRequest);
        }

        /// <summary>
        /// When overridden in derived classes, returns a relayed request, created based on the 
        /// <paramref name="attribute"/> applied to a code element.
        /// </summary>
        /// <param name="request">
        /// An <see cref="ICustomAttributeProvider"/>, which is typically an instance of <see cref="PropertyInfo"/>, 
        /// <see cref="FieldInfo"/> or <see cref="ParameterInfo"/>.
        /// </param>
        /// <param name="attribute">
        /// A <typeparamref name="TAttribute"/> instance applied to the code element.
        /// </param>
        protected abstract object CreateRelayedRequest(ICustomAttributeProvider request, TAttribute attribute);
    }
}
