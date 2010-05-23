using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for many instances and returns the results.
    /// </summary>
    public class ManyUnwrapper : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates many specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// Many specimens if <paramref name="request"/> is a <see cref="ManyRequest"/> instance;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var manyRequest = request as ManyRequest;
            if (manyRequest == null)
            {
                return new NoSpecimen(request);
            }

            return from r in manyRequest.CreateRequests()
                   select container.Resolve(r);
        }

        #endregion
    }
}
