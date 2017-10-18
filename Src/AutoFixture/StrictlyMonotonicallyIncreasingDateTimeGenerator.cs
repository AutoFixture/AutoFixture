using System;
using System.Threading;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="DateTime"/> specimens based on a incremental sequence of days.
    /// </summary>
    public class StrictlyMonotonicallyIncreasingDateTimeGenerator : ISpecimenBuilder
    {
        private readonly DateTime seed;
        private int baseValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrictlyMonotonicallyIncreasingDateTimeGenerator"/> class.
        /// </summary>
        /// <param name="seed">The base <see cref="DateTime"/> value used to generate <see cref="DateTime"/> specimens.</param>
        public StrictlyMonotonicallyIncreasingDateTimeGenerator(DateTime seed)
        {
            this.seed = seed;
        }

        /// <summary>
        /// Creates a new <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="DateTime"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(DateTime).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.seed.AddDays(this.GetNextNumberInSequence());
        }

        private int GetNextNumberInSequence()
        {
            return Interlocked.Increment(ref this.baseValue);
        }
    }
}
