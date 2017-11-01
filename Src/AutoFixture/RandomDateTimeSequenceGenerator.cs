using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates random <see cref="DateTime"/> specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="DateTime"/> values will be within
    /// a range of ± two years from today's date,
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class RandomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateTimeSequenceGenerator"/> class.
        /// </summary>
        public RandomDateTimeSequenceGenerator()
            : this(DateTime.Today.AddYears(-2), DateTime.Today.AddYears(2))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateTimeSequenceGenerator"/> class
        /// for a specific range of dates.
        /// </summary>
        /// <param name="minDate">The lower bound of the date range.</param>
        /// <param name="maxDate">The upper bound of the date range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
        /// </exception>
        public RandomDateTimeSequenceGenerator(DateTime minDate, DateTime maxDate)
        {
            if (minDate >= maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");
            }

            this.randomizer = new RandomNumericSequenceGenerator(minDate.Ticks, maxDate.Ticks);
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
            if (context == null) throw new ArgumentNullException(nameof(context));

            return IsNotDateTimeRequest(request)
                       ? new NoSpecimen()
                       : this.CreateRandomDate(context);
        }

        private static bool IsNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).GetTypeInfo().IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate(ISpecimenContext context)
        {
            return new DateTime(this.GetRandomNumberOfTicks(context));
        }

        private long GetRandomNumberOfTicks(ISpecimenContext context)
        {
            return (long)this.randomizer.Create(typeof(long), context);
        }
    }
}
