using System;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Interface to abstract accessing requested member type
    /// </summary>
    public interface IMemberTypeResolver
    {
        /// <summary>
        /// Tries to determine requested member type and returns it. Otherwise returns null.
        /// </summary>
        /// <param name="request">Relay request object</param>
        /// <returns></returns>
        Type TryGetMemberType(object request);
    }
}