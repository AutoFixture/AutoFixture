namespace AutoFixture.Kernel
{
    /// <summary>
    /// Event arguments about a created specimen.
    /// </summary>
    public class SpecimenCreatedEventArgs : RequestTraceEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecimenCreatedEventArgs"/> class with the
        /// supplied values.
        /// </summary>
        /// <param name="request">A request for a specimen.</param>
        /// <param name="specimen">
        /// The specimen that was created base on <paramref name="request"/>.
        /// </param>
        /// <param name="depth">
        /// The recursion depth at which <paramref name="request"/> occurred.
        /// </param>
        public SpecimenCreatedEventArgs(object request, object specimen, int depth)
            : base(request, depth)
        {
            this.Specimen = specimen;
        }

        /// <summary>
        /// Gets the specimen that was created from <see cref="RequestTraceEventArgs.Request"/>.
        /// </summary>
        public object Specimen { get; }
    }
}
