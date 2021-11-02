using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification which omits the <see cref="Fixture"/> and <see cref="IFixture"/> types.
    /// </summary>
    public class OmitFixtureSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates whether or not the <paramref name="request"/> is for a fixture type.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="false"/> if the <paramref name="request"/> is for a fixture type;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return !(request is Type requestType && (requestType == typeof(Fixture) || requestType == typeof(IFixture)));
        }
    }
}