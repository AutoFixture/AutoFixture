using System.ComponentModel;
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
        /// <returns>The next number in a consequtive sequence.</returns>
        public double CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.d;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object CreateAnonymous(object seed)
        {
            return this.CreateAnonymous();
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consequtive sequence, if <paramref name="request"/> is a request
        /// for a <see cref="System.Double"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request != typeof(double))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }

        #endregion
    }
}
