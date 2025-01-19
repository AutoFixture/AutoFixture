using System;
using System.Reflection;

namespace AutoFixture.Xunit3.Internal
{
    internal static class ReflectionExtensions
    {
        public static Type GetReturnType(this MemberInfo member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));

            return member switch
            {
                MethodInfo methodInfo => methodInfo.ReturnType,
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                _ => throw new ArgumentException("Member is not a method, property, or field.", nameof(member))
            };
        }
    }
}
