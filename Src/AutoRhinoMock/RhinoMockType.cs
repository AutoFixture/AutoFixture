using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.AutoRhinoMock
{
    internal static class RhinoMockType
    {
        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static bool IsMockable(this object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            return type.IsInterface || type.IsAbstract;
        }
    }
}
