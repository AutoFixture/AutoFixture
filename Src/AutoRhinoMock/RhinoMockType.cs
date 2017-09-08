using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    internal static class RhinoMockType
    {
        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static bool IsMockable(this object request)
        {
            var t = request as Type;
            if (t == null)
            {
                return false;
            }

            return (t.IsInterface || t.IsAbstract);
        }
    }
}
