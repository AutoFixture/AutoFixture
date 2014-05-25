namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for a multidimensional array to a jagged array and converts the result
    /// to the desired multidimensional array type.
    /// </summary>
    public class MultidimensionalArrayRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new multidimensional array based on a request.
        /// </summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// A multidimensional array of the requested type if possible; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}