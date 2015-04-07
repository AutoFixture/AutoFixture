using System;

namespace Ploeh.AutoFixture.Kernel
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
        public object Request
        {
            get { return this.request; }
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
            var other = obj as NoSpecimen;
            if (other != null)
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
            return this.Request == null ? 0 : this.Request.GetHashCode();
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
        
            return object.Equals(this.Request, other.Request);
        }
    }
}
