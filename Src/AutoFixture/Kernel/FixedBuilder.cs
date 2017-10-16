namespace AutoFixture.Kernel
{
    /// <summary>
    /// A <see cref="ISpecimenBuilder"/> that always returns the same specimen.
    /// </summary>
    public class FixedBuilder : ISpecimenBuilder
    {
        private readonly object specimen;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedBuilder"/> class.
        /// </summary>
        /// <param name="specimen">
        /// The specimen to return from the <see cref="Create"/> method.
        /// </param>
        public FixedBuilder(object specimen)
        {
            this.specimen = specimen;
        }

        /// <summary>
        /// Returns the same specimen every time.
        /// </summary>
        /// <param name="request">The request that describes what to create. Ignored.</param>
        /// <param name="context">
        /// A context that can be used to create other specimens. Ignored.
        /// </param>
        /// <returns>
        /// The specimen supplied to the instance in the constructor.
        /// </returns>
        /// <seealso cref="FixedBuilder(object)"/>
        public object Create(object request, ISpecimenContext context)
        {
            return this.specimen;
        }
    }
}
