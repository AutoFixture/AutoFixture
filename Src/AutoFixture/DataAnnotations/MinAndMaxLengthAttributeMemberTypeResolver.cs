using System;
using System.Reflection;

namespace AutoFixture.DataAnnotations
{
    public class MinAndMaxLengthAttributeMemberTypeResolver : IMemberTypeResolver
    {
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