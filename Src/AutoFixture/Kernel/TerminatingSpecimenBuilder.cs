using System.Globalization;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Throws an <see cref="ObjectCreationException"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This <see cref="ISpecimenBuilder"/> can be used as the last item in a
    /// <see cref="CompositeSpecimenBuilder"/> to throw an exception instead of letting the
    /// containing builder return a <see cref="NoSpecimen"/> instance when it can't satisfy a
    /// request.
    /// </para>
    /// </remarks>
    public class TerminatingSpecimenBuilder : ISpecimenBuilder
    {
        /// <summary>
        /// Throws an <see cref="ObjectCreationException"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">
        /// A context that can be used to create other specimens. Not used.
        /// </param>
        /// <returns>
        /// This method never returns. It always throws an <see cref="ObjectCreationException"/>.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            throw new ObjectCreationException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "AutoFixture was unable to create an instance from {0}, most likely because it has no public constructor, is an abstract or non-public type.",
                    request));
        }
    }
}
