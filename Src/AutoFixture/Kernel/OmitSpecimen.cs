using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A signal type used to indicate to the Auto-Property feature that a given request should be
    /// ignored, and no further processing performed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This signal type is different from <see cref="NoSpecimen" /> in that it stops further
    /// processing. Where NoSpecimen is simply an indication that a particular
    /// <see cref="ISpecimenBuilder" /> can't provide a value for a request, an OmitSpecimen
    /// instance explicitly indicates that no value will be provided for a request - not even
    /// <see langword="null" />.
    /// </para>
    /// <para>
    /// The OmitSpecimen type is explicitly understood by <see cref="AutoPropertiesCommand" />,
    /// but can be returned by any ISpecimenBuilder.
    /// </para>
    /// </remarks>
    public class OmitSpecimen : IEquatable<OmitSpecimen>
    {
        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this
        /// instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object" /> to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified <see cref="object" /> is an instance of
        /// <see cref="OmitSpecimen" />; otherwise, <see langword="false"/>.
        /// </returns>
        /// <seealso cref="Equals(OmitSpecimen)" />
        public override bool Equals(object obj)
        {
            if (obj is OmitSpecimen other)
                return this.Equals(other);

            return base.Equals(obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="OmitSpecimen" /> instance is equal to the
        /// current instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="other" /> is not <see langword="null" />;
        /// otherwise <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// It may seem a little strange that any <see cref="OmitSpecimen" /> instance is equal to
        /// any other instance of that type, but the class is a pure signal type that carries no
        /// state, so there's no reason to distinguish between instances. In essence, it could just
        /// as well have been modeled as a Singleton, but wasn't because it seems to bring no
        /// particular extra benefit.
        /// </para>
        /// </remarks>
        public bool Equals(OmitSpecimen other)
        {
            return other != null;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return 1;
        }
    }
}
