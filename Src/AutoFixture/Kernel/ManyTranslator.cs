using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Translates requests for many (an unspecified count) specimens into a request for a specific
    /// number of specimens.
    /// </summary>
    public class ManyTranslator : ISpecimenBuilder
    {
        private int count;

        /// <summary>
        /// Gets or sets the count that specifies how many specimens will be requested.
        /// </summary>
        public int Count
        {
            get { return this.count; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Count cannot be zero or negative.");
                }
                this.count = value;
            }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates many new specimens based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimens if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The number of specimens requested is determined by <see cref="Count"/>.
        /// </para>
        /// </remarks>
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

            return container.Resolve(new FiniteSequenceRequest(manyRequest.Request, this.Count));
        }

        #endregion
    }
}
