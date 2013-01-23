﻿using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class UInt32SequenceGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private uint u;

        /// <summary>
        /// Initializes a new instance of the <see cref="UInt32SequenceGenerator"/> class.
        /// </summary>
        public UInt32SequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        [CLSCompliant(false)]
        public uint CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.u;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consequtive sequence, if <paramref name="request"/> is a request
        /// for an unsigned integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(uint).Equals(request))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }
    }
}
