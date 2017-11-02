﻿using System;
using AutoFixture.Kernel;

namespace AutoFixture
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
        /// <returns>The next number in a consecutive sequence.</returns>
        [CLSCompliant(false)]
        [Obsolete("Please move over to using Create() as this method will be removed in the next release", true)]
        public uint CreateAnonymous()
        {
            return (uint)this.Create(typeof(uint), null);
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
        /// for an unsigned integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(uint).Equals(request))
            {
                return new NoSpecimen();
            }
            
            lock (this.syncRoot)
            {
                return ++this.u;
            }
        }
    }
}
