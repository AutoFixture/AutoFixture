using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates string values based on a supplied factory.
    /// </summary>
    public class StringGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringGenerator"/> class with the supplied
        /// specimen factory.
        /// </summary>
        /// <param name="specimenFactory">
        /// A factory that creates a anonymous variables (specimens). The
        /// <see cref="StringGenerator"/> instance will call <see cref="object.ToString()"/> on the
        /// created specimen.
        /// </param>
        public StringGenerator(Func<object> specimenFactory)
        {
            this.Factory = specimenFactory ?? throw new ArgumentNullException(nameof(specimenFactory));
        }

        /// <summary>
        /// Gets the factory used to specimens.
        /// </summary>
        /// <seealso cref="StringGenerator(Func{object})"/>
        public Func<object> Factory { get; }

        /// <summary>
        /// Creates string specimens by invoking the supplied specimen factory and calling
        /// <see cref="object.ToString()"/> on the result.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The string representation of a specimen created by the contained specimen factory.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(string).Equals(request))
            {
                return new NoSpecimen();
            }

            var specimen = this.Factory();
            if (specimen == null)
            {
                return new NoSpecimen();
            }
            if (specimen is NoSpecimen)
            {
                return specimen;
            }
            return specimen.ToString();
        }
    }
}
