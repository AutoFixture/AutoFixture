using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates random <see cref="DateTime"/> specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="DateTime"/> values will be between
    /// <see cref="DateTime.MinValue"/> and <see cref="DateTime.MaxValue"/>
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class RandomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly DateTime minDate;
        private readonly DateTime maxDate;
        private readonly Random randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateTimeSequenceGenerator"/> class.
        /// </summary>
        public RandomDateTimeSequenceGenerator()
            : this(DateTime.MinValue, DateTime.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateTimeSequenceGenerator"/> class
        /// for a specific range of dates.
        /// </summary>
        /// <param name="minDate">The lower bound of the date range.</param>
        /// <param name="maxDate">The uppder bound of the date range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
        /// </exception>
        public RandomDateTimeSequenceGenerator(DateTime minDate, DateTime maxDate)
        {
            if (minDate > maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than or equal to the 'maxDate'.");
            }

            this.minDate = minDate;
            this.maxDate = maxDate;
            this.randomizer = new Random();
        }

        /// <summary>
        /// Creates a new <see cref="DateTime"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="DateTime"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (IsNotDateTimeRequest(request))
            {
                return new NoSpecimen(request);
            }

            return CreateRandomDate();
        }

        private static bool IsNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate()
        {
            return new DateTime(this.GetRandomNumberOfTicks());
        }

        private long GetRandomNumberOfTicks()
        {
            var number = this.GetRandomLongPositiveNumber();
            return ReduceNumberToTicksRange(number);
        }

        private ulong GetRandomLongPositiveNumber()
        {
            var buffer = new byte[8];
            this.randomizer.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        private long ReduceNumberToTicksRange(ulong number)
        {
            var minTicks = (ulong)this.minDate.Ticks;
            var maxTicks = (ulong)this.maxDate.Ticks;
            var ticks = ((number % (maxTicks - minTicks + 1)) + minTicks);
            return (long)ticks;
        }
    }
}
