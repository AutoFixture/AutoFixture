using System;
using System.Threading;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class Int64SequenceGenerator : ISpecimenBuilder
    {
        private long l;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int64SequenceGenerator"/> class.
        /// </summary>
        public Int64SequenceGenerator()
        {
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        public long Create()
        {
            return Interlocked.Increment(ref this.l);
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <remarks>Obsolete: Please move over to using <see cref="Create()">Create()</see> as this method will be removed in the next release</remarks>
        /// <returns>The next number in a consecutive sequence.</returns>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release")]
        public long CreateAnonymous()
        {
            return Create();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
        /// for an 64-bit integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(long).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.Create();
        }
    }
}
