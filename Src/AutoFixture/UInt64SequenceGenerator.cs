using System;
using AutoFixture.Kernel;

namespace AutoFixture
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
        /// <returns>The next number in a consecutive sequence.</returns>
        [CLSCompliant(false)]
        [Obsolete("Please move over to using Create() as this method will be removed in the next release", true)]
        public ulong CreateAnonymous()
        {
            return (ulong)this.Create(typeof(ulong), null);
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
        /// for an unsigned 64-bit integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(ulong).Equals(request))
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
