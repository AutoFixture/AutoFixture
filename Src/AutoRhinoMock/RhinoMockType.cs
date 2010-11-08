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
    }
}
