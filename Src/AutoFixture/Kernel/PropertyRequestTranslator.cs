using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Translates a request for a property into a <see cref="SeededRequest"/> with a seed based
    /// on the property's name.
    /// </summary>
    public class PropertyRequestTranslator : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a specimen based on a requested property.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="SeededRequest"/> encapsulating the property type
        /// and name of the requested property, if possible; otherwise, a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var propertyInfo = request as PropertyInfo;
            if (propertyInfo == null)
            {
                return new NoSpecimen(request);
            }

            return container.Resolve(new SeededRequest(propertyInfo.PropertyType, propertyInfo.Name));
        }

        #endregion
    }
}
