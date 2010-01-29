using System.ComponentModel;
using System.Threading;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class Int64SequenceGenerator
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
        /// <returns>The next number in a consequtive sequence.</returns>
        public long CreateAnonymous()
        {
            return Interlocked.Increment(ref this.l);
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
