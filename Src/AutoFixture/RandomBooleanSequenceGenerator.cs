using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates random value <see langword="true"/> or <see langword="false"/>.
    /// </summary>
    public class RandomBooleanSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomBooleanNumbers;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RandomBooleanSequenceGenerator"/> class.
        /// </summary>
        public RandomBooleanSequenceGenerator()
        {
            this.randomBooleanNumbers = new RandomNumericSequenceGenerator(0, 1);
        }

        /// <summary>
        /// Returns <see langword="true"/> or <see langword="false"/> randomly.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// <see langword="true"/> or <see langword="false"/> generated randomly using <see cref="Random"/>,
        /// if <paramref name="request"/> is a request for a boolean; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(bool).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.GenerateBoolean(context);
        }

        private bool GenerateBoolean(ISpecimenContext context)
        {
            return (int)this.randomBooleanNumbers.Create(typeof(int), context) == 0;
        }
    }
}
