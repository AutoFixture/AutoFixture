﻿using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class SingleSequenceGenerator : ISpecimenBuilder
    {
        private float f;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleSequenceGenerator"/> class.
        /// </summary>
        public SingleSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        public float CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.f;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consequtive sequence, if <paramref name="request"/> is a request
        /// for a <see cref="System.Single"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(float).Equals(request))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }
    }
}
