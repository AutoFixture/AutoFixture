using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    public static class RhinoMockType
    {
        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static bool IsMockable(object request)
        {
            var t = request as Type;
            if(t == null)
            {
                return false;
            }

            if (t.IsInterface || t.IsAbstract)
            {
                return true;
            }

            return false;
        }

        public static bool IsGeneric(Type t)
        {
            if (t == null)
            {
                return false;
            }

            if (t.IsGenericType && RhinoMockType.IsMockable(t.GetGenericTypeDefinition()))
            {
                return true;
            }

            return false;
        }
    }
}
