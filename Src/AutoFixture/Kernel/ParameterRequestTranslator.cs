using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Translates a request for a parameter into a <see cref="SeededRequest"/> with a seed based
    /// on the parameter's name.
    /// </summary>
    public class ParameterRequestTranslator : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a specimen based on a requested parameter.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="SeededRequest"/> encapsulating the parameter type
        /// and name of the requested parameter, if possible; otherwise, a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var paramInfo = request as ParameterInfo;
            if (paramInfo == null)
            {
                return new NoSpecimen(request);
            }

            return container.Resolve(new SeededRequest(paramInfo.ParameterType, paramInfo.Name));
        }

        #endregion
    }
}
