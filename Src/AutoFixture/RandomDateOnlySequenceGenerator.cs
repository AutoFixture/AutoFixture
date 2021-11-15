#if SYSTEM_DATEONLY

using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates random <see cref="DateOnly"/> specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="DateOnly"/> values will be within
    /// a range of ± two years from today's date,
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class RandomDateOnlySequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateOnlySequenceGenerator"/> class.
        /// </summary>
        public RandomDateOnlySequenceGenerator()
            : this(DateOnly.FromDateTime(DateTime.Today).AddYears(-2),
                   DateOnly.FromDateTime(DateTime.Today).AddYears(2))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomDateOnlySequenceGenerator"/> class
        /// for a specific range of dates.
        /// </summary>
        /// <param name="minDate">The lower bound of the date range.</param>
        /// <param name="maxDate">The upper bound of the date range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
        /// </exception>
        public RandomDateOnlySequenceGenerator(DateOnly minDate, DateOnly maxDate)
        {
            if (minDate >= maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");
            }

            this.randomizer = new RandomNumericSequenceGenerator(minDate.DayNumber, maxDate.DayNumber);
        }

        /// <summary>
        /// Creates a new <see cref="DateOnly"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateOnly"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="DateOnly"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return IsNotDateOnlyRequest(request)
                ? new NoSpecimen()
                : this.CreateRandomDate(context);
        }

        private static bool IsNotDateOnlyRequest(object request)
        {
            return !typeof(DateOnly).GetTypeInfo().IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate(ISpecimenContext context)
        {
            return DateOnly.FromDayNumber(this.GetRandomNumberOfDays(context));
        }

        private int GetRandomNumberOfDays(ISpecimenContext context)
        {
            return (int)this.randomizer.Create(typeof(int), context);
        }
    }
}

#endif