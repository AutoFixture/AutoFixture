using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for a property to a <see cref="SeededRequest"/> with a seed based
    /// on the property's name.
    /// </summary>
    public class PropertyRequestRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a specimen based on a requested property.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="SeededRequest"/> encapsulating the property type
        /// and name of the requested property, if possible; otherwise, a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var propertyInfo = request as PropertyInfo;
            if (propertyInfo == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(new SeededRequest(propertyInfo.PropertyType, propertyInfo.Name));
        }
    }
}
