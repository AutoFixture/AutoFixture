using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class UInt64SequenceGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private ulong u;

        /// <summary>
        /// Initializes a new instance of the <see cref="UInt64SequenceGenerator"/> class.
        /// </summary>
        public UInt64SequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        [CLSCompliant(false)]
        public ulong CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.u;
            }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consequtive sequence, if <paramref name="request"/> is a request
        /// for an unsigned 64-bit integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request != typeof(ulong))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }

        #endregion
    }
}
