using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    internal static class MethodInfoExtensions
    {
        internal static bool IsEqualsMethod(this MethodInfo method)
        {
            return method.Name == "Equals"
                && method.GetParameters().Length == 1
                && method.ReturnType == typeof(bool);
        }

        internal static bool IsGetHashCodeMethod(this MethodInfo method)
        {
            return method.Name == "GetHashCode"
                && method.GetParameters().Length == 0
                && method.ReturnType == typeof(int);
        }

        internal static bool IsToString(this MethodInfo method)
        {
            return method.Name == "ToString"
                   && method.GetParameters().Length == 0
                   && method.ReturnType == typeof(string);
        }

        internal static bool IsGetType(this MethodInfo method)
        {
            return method.Name == "GetType"
                   && method.GetParameters().Length == 0
                   && method.ReturnType == typeof(Type);
        }

        /// <summary>
        /// Gets a value that indicates if the method is the <see cref="object.Equals(object)"/>
        /// method declared on the <see cref="System.Object"/> type.
        /// </summary>
        internal static bool IsObjectEqualsMethod(this MethodInfo method)
        {
            return method.DeclaringType == typeof(object) && method.IsEqualsMethod();
        }

        /// <summary>
        /// Gets a value that indicates if the method is the <see cref="object.GetHashCode()"/>
        /// method declared on the <see cref="System.Object"/> type.
        /// </summary>
        internal static bool IsObjectGetHashCodeMethod(this MethodInfo method)
        {
            return method.DeclaringType == typeof(object) && method.IsGetHashCodeMethod();
        }

        /// <summary>
        /// Gets a value that indicates if the method is an override of the
        /// <see cref="object.Equals(object)"/> method.
        /// </summary>
        internal static bool IsObjectEqualsOverrideMethod(this MethodInfo method)
        {
            return method.IsEqualsMethod() && !method.IsObjectEqualsMethod();
        }

        /// <summary>
        /// Gets a value that indicates if the method is an override of the
        /// <see cref="object.GetHashCode()"/> method.
        /// </summary>
        internal static bool IsObjectGetHashCodeOverrideMethod(this MethodInfo method)
        {
            return method.IsGetHashCodeMethod() && !method.IsObjectGetHashCodeMethod();
        }
    }
}
