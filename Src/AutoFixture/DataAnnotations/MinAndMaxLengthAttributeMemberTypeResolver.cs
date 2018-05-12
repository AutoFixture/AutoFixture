using System;
using System.Reflection;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Member type resolver used by MinAndMaxLengthAttributeRelay
    /// </summary>
    public class MinAndMaxLengthAttributeMemberTypeResolver : IMemberTypeResolver
    {
        /// <summary>
        /// Tries to determine requested member type and returns it. Otherwise returns null.
        /// </summary>
        /// <param name="request">Relay request object</param>
        /// <returns></returns>
        public Type TryGetMemberType(object request)
        {
            Type conversionType;
            switch (request)
            {
                case PropertyInfo pi:
                    conversionType = pi.PropertyType;
                    break;

                case FieldInfo fi:
                    conversionType = fi.FieldType;
                    break;

                case ParameterInfo pi:
                    conversionType = pi.ParameterType;
                    break;

                default:
                    return null;
            }

            var underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                conversionType = underlyingType;
            }

            return conversionType;
        }
    }
}