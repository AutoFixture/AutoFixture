using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a seed for a given type.
    /// </summary>
    public class SeededRequest : IEquatable<SeededRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Seed"/> class.
        /// </summary>
        /// <param name="request">The request for which the seed applies.</param>
        /// <param name="seed">The seed.</param>
        public SeededRequest(object request, object seed)
        {
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Seed = seed;
        }

        /// <summary>
        /// Gets the seed value.
        /// </summary>
        public object Seed { get; }

        /// <summary>
        /// Gets the inner request for which the seed applies.
        /// </summary>
        public object Request { get; }

        /// <summary>
        /// Determines whether this instance equals another instance.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance equals <paramref name="obj"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is SeededRequest other)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for the instance.
        /// </summary>
        /// <returns>The hash code for the instance.</returns>
        public override int GetHashCode()
        {
            return this.Request.GetHashCode() ^ (this.Seed == null ? 0 : this.Seed.GetHashCode());
        }

        /// <summary>
        /// Determines whether this instance equals another instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance equals <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(SeededRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Request == other.Request
                && object.Equals(this.Seed, other.Seed);
        }
    }
}
