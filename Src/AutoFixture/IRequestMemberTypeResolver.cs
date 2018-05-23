using System;

namespace AutoFixture
{
    /// <summary>
    /// Resolver of the member type for the class member requests (e.g. PropertyInfo, FieldInfo)
    /// </summary>
    public interface IRequestMemberTypeResolver
    {
        /// <summary>
        /// Tries to determine requested member type and returns it.
        /// </summary>
        /// <param name="request">Relay request object</param>
        /// <param name="memberType">Output variable for resolved request member type</param>
        /// <returns></returns>
        bool TryGetMemberType(object request, out Type memberType);
    }
}