using System;

namespace AutoFixture
{
    /// <summary>
    /// Resolver of the member type for the class member requests (e.g. PropertyInfo, FieldInfo).
    /// </summary>
    public interface IRequestMemberTypeResolver
    {
        /// <summary>
        /// Tries to determine requested member type and returns it.
        /// </summary>
        bool TryGetMemberType(object request, out Type memberType);
    }
}