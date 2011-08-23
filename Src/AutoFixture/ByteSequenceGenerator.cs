using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class ByteSequenceGenerator : ISpecimenBuilder
    {
        private byte b;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteSequenceGenerator"/> class.
        /// </summary>
        public ByteSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        public byte CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.b;
            }
        }

        /// <summary>
        /// Creates an anonymous byte.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next byte in a consequtive sequence, if <paramref name="request"/> is a request
        /// for a byte; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request != typeof(byte))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }
    }
}
