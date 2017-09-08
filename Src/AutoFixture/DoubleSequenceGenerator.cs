using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class DoubleSequenceGenerator : ISpecimenBuilder
    {
        private double d;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleSequenceGenerator"/> class.
        /// </summary>
        public DoubleSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        public double Create()
        {
            lock (this.syncRoot)
            {
                return ++this.d;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        /// <remarks>Obsolete: Please move over to using <see cref="Create()">Create()</see> as this method will be removed in the next release</remarks>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release")]
        public double CreateAnonymous()
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
        /// for a <see cref="System.Double"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(double).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.Create();
        }
    }
}
