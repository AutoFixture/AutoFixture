using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal static class NSubstituteType
    {
        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}