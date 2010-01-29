using System.ComponentModel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class SingleSequenceGenerator
    {
        private float f;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleSequenceGenerator"/> class.
        /// </summary>
        public SingleSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        public float CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.f;
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
    }
}
