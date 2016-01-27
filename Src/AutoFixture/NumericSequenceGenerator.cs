using System;
using System.Threading;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class NumericSequenceGenerator : ISpecimenBuilder
    {
        private long value = 0;

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            return this.CreateNumericSpecimen(type);
        }

        private object CreateNumericSpecimen(Type request)
        {
            if(!request.IsNumeric())
            {
                return new NoSpecimen();
            }
            var random = GetNextNumber();
            try
            {
                return Convert.ChangeType(random, request);
            }
            catch(OverflowException)
            {
                return Convert.ChangeType(0, request);
            }
        }

        private long GetNextNumber()
        {
            return Interlocked.Increment(ref this.value);
        }
    }
}
