using System;
using System.Globalization;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a sequence of random printable ASCII characters (Dec 33-126).
    /// </summary>
    public class RandomCharSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomPrintableCharNumbers;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RandomCharSequenceGenerator"/> class.
        /// </summary>
        public RandomCharSequenceGenerator()
        {
            this.randomPrintableCharNumbers = new RandomNumericSequenceGenerator(33, 126);
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.
        /// </param>
        /// <param name="context">A context that can be used to create other
        /// specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(char).Equals(request))
                return new NoSpecimen();

            return Convert.ToChar(
                this.randomPrintableCharNumbers.Create(typeof(int), context),
                CultureInfo.CurrentCulture);
        }
    }
}
