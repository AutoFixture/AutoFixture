using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class DecimalSequenceGenerator : ISpecimenBuilder
    {
        private decimal d;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int64SequenceGenerator"/> class.
        /// </summary>
        public DecimalSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        public decimal CreateAnonymous()
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
        /// <param name="container">Not used.</param>
        /// <returns>The next number in a consequtive sequence.</returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (request != typeof(decimal))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }

        #endregion
    }
}
