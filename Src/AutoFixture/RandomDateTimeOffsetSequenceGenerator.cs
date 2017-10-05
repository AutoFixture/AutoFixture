using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates random <see cref="DateTimeOffset"/> specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="DateTimeOffset"/> UTC values will be within
    /// a range of ± two years from today's date,
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class RandomDateTimeOffsetSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomDateTimeSequenceGenerator _dateRandomizer;
        private readonly RandomNumericSequenceGenerator _offsetRandomizer;

        // Offsets have a maximum ± of 14hrs...
        private static long MinutesInFourteenHours = 840L;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateTimeOffsetSequenceGenerator"/> class.
        /// </summary>
        public RandomDateTimeOffsetSequenceGenerator()
            : this(DateTimeOffset.Now.AddYears(-2), DateTimeOffset.Now.AddYears(2))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateTimeOffsetSequenceGenerator"/> class
        /// for a specific range of dates.
        /// </summary>
        /// <param name="minDate">The lower bound of the date range.</param>
        /// <param name="maxDate">The upper bound of the date range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
        /// </exception>
        public RandomDateTimeOffsetSequenceGenerator(DateTimeOffset minDate, DateTimeOffset maxDate)
        {
            if (minDate >= maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");
            }

            _dateRandomizer = new RandomDateTimeSequenceGenerator(minDate.ToUniversalTime().DateTime, maxDate.ToUniversalTime().DateTime);
            _offsetRandomizer = new RandomNumericSequenceGenerator(MinutesInFourteenHours * -1, MinutesInFourteenHours);
        }

        /// <summary>
        /// Creates a new <see cref="DateTimeOffset"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateTimeOffset"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="DateTimeOffset"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return IsNotDateTimeOffsetRequest(request)
                ? new NoSpecimen()
                : CreateRandomDate(context);
        }

        private static bool IsNotDateTimeOffsetRequest(object request)
        {
            return !typeof(DateTimeOffset).IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate(ISpecimenContext context)
        {
            // This ensures that the date/time falls within the specified range
            var utcDto = new DateTimeOffset(GetRandomNumberOfTicks(context), TimeSpan.Zero);
            // This then applies a randomly generated offset
            return utcDto.ToOffset(GetRandomOffset(context));
        }

        private TimeSpan GetRandomOffset(ISpecimenContext context)
        {
            return new TimeSpan((long)_offsetRandomizer.Create(typeof(long), context) * TimeSpan.TicksPerMinute);
        }

        private DateTime GetRandomNumberOfTicks(ISpecimenContext context)
        {
            return (DateTime)this._dateRandomizer.Create(typeof(DateTime), context);
        }
    }
}