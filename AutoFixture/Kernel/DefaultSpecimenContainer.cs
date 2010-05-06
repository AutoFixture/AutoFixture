using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Default implementation of <see cref="ISpecimenContainer"/>.
    /// </summary>
    public class DefaultSpecimenContainer : ISpecimenContainer
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultSpecimenContainer"/> with the supplied
        /// <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder that will handle requests.</param>
        public DefaultSpecimenContainer(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the <see cref="ISpecimenBuilder"/> contained by the instance.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        #region ISpecimenContainer Members

        /// <summary>
        /// Creates an anonymous variable (specimen) based on a request by delegating the request
        /// to its contained <see cref="Builder"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <returns>The result of a request to the contained <see cref="Builder"/>.</returns>
        public object Resolve(object request)
        {
            return this.Builder.Create(request, this);
        }

        #endregion
    }
}
