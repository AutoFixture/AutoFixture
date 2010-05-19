using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new specimen from a <see cref="SeededRequest"/> using a function.
    /// </summary>
    /// <typeparam name="T">The type of specimen to create.</typeparam>
    public class SeededFactory<T> : ISpecimenBuilder
    {
        private readonly Func<T, T> create;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeededFactory&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="factory">The function that will create the specimen from a seed.</param>
        public SeededFactory(Func<T, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.create = factory;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a seeded request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The result of the contained function if <paramref name="request"/> is a
        /// <see cref="SeededRequest"/> for the correct type and containing a seed of the same
        /// type; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            var seededRequest = request as SeededRequest;
            if (seededRequest == null)
            {
                return new NoSpecimen(request);
            }

            if (seededRequest.Request != typeof(T))
            {
                return new NoSpecimen(request);
            }

            if (!(seededRequest.Seed is T))
            {
                return new NoSpecimen(request);
            }
            var seed = (T)seededRequest.Seed;

            return this.create(seed);
        }

        #endregion
    }
}
