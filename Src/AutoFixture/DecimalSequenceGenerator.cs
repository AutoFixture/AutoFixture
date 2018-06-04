using System;
using AutoFixture.Kernel;

namespace AutoFixture
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
        [Obsolete("Please use the Create(request, context) method as this overload will be removed to make API uniform.")]
        public decimal Create()
        {
            lock (this.syncRoot)
            {
                return ++this.d;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release", true)]
        public decimal CreateAnonymous()
        {
            return this.Create();
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(decimal).Equals(request))
            {
                return new NoSpecimen();
            }

#pragma warning disable 618
            return this.Create();
#pragma warning restore 618
        }
    }
}
