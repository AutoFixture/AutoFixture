using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Translates a request for a field into a <see cref="SeededRequest"/> with a seed based
    /// on the field's name.
    /// </summary>
    public class FieldRequestTranslator : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a requested field.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="SeededRequest"/> encapsulating the field type
        /// and name of the requested field, if possible; otherwise, a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var fieldInfo = request as FieldInfo;
            if (fieldInfo == null)
            {
                return new NoSpecimen(request);
            }

            return container.Create(new SeededRequest(fieldInfo.FieldType, fieldInfo.Name));
        }

        #endregion
    }
}
