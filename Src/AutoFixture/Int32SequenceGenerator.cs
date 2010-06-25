using System.ComponentModel;
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
        /// <returns>The next number in a consequtive sequence.</returns>
        public int CreateAnonymous()
        {
            return Interlocked.Increment(ref this.i);
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
        /// for an integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request != typeof(int))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }

        #endregion
    }
}
