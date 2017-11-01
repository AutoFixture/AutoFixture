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
        private readonly object request;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimen"/> class with a null request.
        /// </summary>
        public NoSpecimen()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimen"/> class with the supplied
        /// request.
        /// </summary>
        /// <param name="request">
        /// The original request that prompts the creation of this instance.
        /// </param>
        [Obsolete("Use NoSpecimen() instead of NoSpecimen(object). The Request property, and this constructor that populates it, is being retired in future versions of AutoFixture, as it has turned out that no one uses it. You can still use the parameterless NoSpecimen constructor overload. The NoSpecimen class itself will remain. If you're seeing this warning in AutoFixture 3.x, and, despite expectations, have a real need to use the Request property, please provide feedback on https://github.com/AutoFixture/AutoFixture/issues/475 .", true)]
        public NoSpecimen(object request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the original request that prompted the creation of the current instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property value may be <see langword="null"/>.
        /// </para>
        /// </remarks>
        [Obsolete("The Request property is being retired in future versions of AutoFixture, as it has turned out that no one uses it. If you're seeing this warning in AutoFixture 3.x, and, despite expectations, have a real need to use the Request property, please provide feedback on https://github.com/AutoFixture/AutoFixture/issues/475.", true)]
        public object Request => this.request;

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
#pragma warning disable 618
            return this.request == null ? 0 : this.request.GetHashCode();
#pragma warning restore 618
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

#pragma warning disable 618
            return object.Equals(this.request, other.request);
#pragma warning restore 618
        }
    }
}
