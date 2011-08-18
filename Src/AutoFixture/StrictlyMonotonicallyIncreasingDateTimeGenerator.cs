using System;
using System.Threading;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="DateTime"/> specimens based on a incremental sequence of days.
    /// </summary>
    public class StrictlyMonotonicallyIncreasingDateTimeGenerator : ISpecimenBuilder
    {
        private int baseValue;

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
            if (request != typeof(DateTime))
            {
                return new NoSpecimen(request);
            }

            return DateTime.Now.AddDays(this.GetNextNumberInSequence());
        }

        private int GetNextNumberInSequence()
        {
            return Interlocked.Increment(ref this.baseValue);
        }
    }
}