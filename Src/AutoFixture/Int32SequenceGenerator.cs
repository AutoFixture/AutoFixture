using System;
using System.Threading;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class Int32SequenceGenerator : ISpecimenBuilder
    {
        private int i;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int32SequenceGenerator"/> class.
        /// </summary>
        public Int32SequenceGenerator()
        {
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        public int Create()
        {
            return Interlocked.Increment(ref this.i);
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <remarks>Obsolete: Please move over to using <see cref="Create()">Create()</see> as this method will be removed in the next release</remarks>
        /// <returns>The next number in a consecutive sequence.</returns>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release")]
        public int CreateAnonymous()
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
        /// for an integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(int).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.Create();
        }
    }
}
