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
    }
}
