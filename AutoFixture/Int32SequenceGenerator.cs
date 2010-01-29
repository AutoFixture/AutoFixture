using System.ComponentModel;
using System.Threading;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class Int32SequenceGenerator
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
    }
}
