using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays requests made for properties, fields or parameters to different requests, based on custom attributes 
    /// applied to the code elements. 
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
    public class AttributeRelay<TAttribute> : ISpecimenBuilder
        where TAttribute : Attribute
    {
        private readonly Func<ICustomAttributeProvider, TAttribute, object> createRelayedRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeRelay{TAttribute}"/> class.
        /// </summary>
        /// <param name="createRelayedRequest">
        /// A delegate responsible for creating a relayed request based on an <see cref="Attribute"/> applied to to the 
        /// code element represented by an <see cref="ICustomAttributeProvider"/>.
        /// </param>
        public AttributeRelay(Func<ICustomAttributeProvider, TAttribute, object> createRelayedRequest)
        {
            if (createRelayedRequest == null)
            {
                throw new ArgumentNullException("createRelayedRequest");
            }

            this.createRelayedRequest = createRelayedRequest;
        }

        /// <summary>
        /// Gets a <see cref="MethodInfo"/> representing the delegate specified in the constructor.
        /// </summary>
        /// <remarks>
        /// This property is meant to be used in structural inspection tests for classes that use the
        /// <see cref="AttributeRelay{T}"/>. It returns a <see cref="MethodInfo"/> instead of the actual
        /// delegate to clearly state that the delegate is not meant to be invoked directly by this class'
        /// consumers.
        /// </remarks>
        public MethodInfo CreateRelayedRequestMethod 
        {
            get { return this.createRelayedRequest.Method; }
        }

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

            object relayedRequest = this.createRelayedRequest(customAttributeProvider, attribute);
            return context.Resolve(relayedRequest);
        }
    }
}
