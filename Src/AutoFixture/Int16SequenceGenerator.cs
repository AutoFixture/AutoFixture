using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class Int16SequenceGenerator : ISpecimenBuilder
    {
        private short s;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int16SequenceGenerator"/> class.
        /// </summary>
        public Int16SequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        [Obsolete("Please use the Create(request, context) method as this overload will be removed to make API uniform.")]
        public short Create()
        {
            lock (this.syncRoot)
            {
                return ++this.s;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        /// /// <remarks>Obsolete: Please move over to using <see cref="Create()">Create()</see> as this method will be removed in the next release.</remarks>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release", true)]
        public short CreateAnonymous()
        {
            return this.Create();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
        /// for a 16-bit integer; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(short).Equals(request))
            {
                return new NoSpecimen();
            }

#pragma warning disable 618
            return this.Create();
#pragma warning restore 618
        }
    }
}
