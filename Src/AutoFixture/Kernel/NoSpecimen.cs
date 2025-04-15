using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Signifies that it's not a specimen.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="ISpecimenBuilder"/> implementations are expected to return
    /// <see cref="NoSpecimen"/> instances if they can't handle the request. This ensures that
    /// <see langword="null"/> can be used as a proper return value.
    /// </para>
    /// </remarks>
    public class NoSpecimen : IEquatable<NoSpecimen>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimen"/> class with a null request.
        /// </summary>
        public NoSpecimen()
        {
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current
        /// <see cref="NoSpecimen"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare to the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="object"/> is equal to the current
        /// instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is NoSpecimen other)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for the <see cref="NoSpecimen"/> class.
        /// </summary>
        /// <returns>A hash code for the current <see cref="NoSpecimen"/> instance.</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Indicates whether the current instance is equal to another <see cref="NoSpecimen"/>
        /// instance.
        /// </summary>
        /// <param name="other">
        /// A <see cref="NoSpecimen"/> instance to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current instance is equal to the <paramref name="other"/>
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(NoSpecimen other)
        {
            if (other == null)
            {
                return false;
            }
        }
    }
}
