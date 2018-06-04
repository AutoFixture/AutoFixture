using System;
using System.Reflection;

namespace AutoFixture
{
    /// <summary>
    /// Resolver of the member type for the class member requests (e.g. PropertyInfo, FieldInfo).
    /// </summary>
    public class RequestMemberTypeResolver : IRequestMemberTypeResolver
    {
        /// <summary>
        /// Tries to determine requested member type and returns it.
        /// </summary>
        /// <param name="request">Relay request object.</param>
        /// <param name="memberType">Output variable for resolved request member type.</param>
        /// <returns></returns>
        public bool TryGetMemberType(object request, out Type memberType)
        {
            memberType = null;

            switch (request)
            {
                case PropertyInfo pi:
                    memberType = pi.PropertyType;
                    break;

                case FieldInfo fi:
                    memberType = fi.FieldType;
                    break;

                case ParameterInfo pi:
                    memberType = pi.ParameterType;
                    break;

                default:
                    return false;
            }

            memberType = Nullable.GetUnderlyingType(memberType) ?? memberType;
            return true;
        }
    }
}