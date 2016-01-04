﻿using System;

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
                throw new ArgumentNullException(nameof(factory));
            }

            this.create = factory;
        }

        /// <summary>
        /// Gets the factory that is used to create specimens from a seed.
        /// </summary>
        public Func<T, T> Factory
        {
            get { return this.create; }
        }

        /// <summary>
        /// Creates a new specimen based on a seeded request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The result of the contained function if <paramref name="request"/> is a
        /// <see cref="SeededRequest"/> for the correct type and containing a seed of the same
        /// type; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request != null && request.Equals(typeof(T)))
            {
                return this.create(default(T));
            }

            var seededRequest = request as SeededRequest;
            if (seededRequest == null)
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            if (!seededRequest.Request.Equals(typeof(T)))
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            if ((seededRequest.Seed != null)
                && !(seededRequest.Seed is T))
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }
            var seed = (T)seededRequest.Seed;

            return this.create(seed);
        }
    }
}
