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

        /// <summary>
        /// Gets a value that indicates if the method is the <see cref="object.Equals(object)"/>
        /// method declared on the <see cref="System.Object"/> type.
        /// </summary>
        internal static bool IsObjectEqualsMethod(this MethodInfo method)
        {
            return method.DeclaringType == typeof(object) && method.IsEqualsMethod();
        }

        /// <summary>
        /// Gets a value that indicates if the method is an override of the
        /// <see cref="object.Equals(object)"/> method.
        /// </summary>
        internal static bool IsObjectEqualsOverrideMethod(this MethodInfo method)
        {
            return method.IsEqualsMethod() && !method.IsObjectEqualsMethod();
        }
    }
}
