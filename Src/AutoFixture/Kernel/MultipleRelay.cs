using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays requests for multiple (an unspecified count) specimens to a request for a specific
    /// number of specimens.
    /// </summary>
    public class MultipleRelay : ISpecimenBuilder
    {
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleRelay"/> class.
        /// </summary>
        public MultipleRelay()
        {
            this.count = 3;
        }

        /// <summary>
        /// Gets or sets the count that specifies how many specimens will be requested.
        /// </summary>
        public int Count
        {
            get => this.count;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Count cannot be negative.");
                }
                this.count = value;
            }
        }

        /// <summary>
        /// Creates many new specimens based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimens if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The number of specimens requested is determined by <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var manyRequest = request as MultipleRequest;
            if (manyRequest == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(new FiniteSequenceRequest(manyRequest.Request, this.Count));
        }
    }
}
