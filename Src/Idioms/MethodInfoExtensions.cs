using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    internal static class MethodInfoExtensions
    {
        private static readonly MethodInfo equalsMethod = typeof(object).GetMethod("Equals", new[] { typeof(object) });

        internal static bool IsEqualsMethod(this MethodInfo method)
        {
            return MethodInfoExtensions.equalsMethod.Equals(method.GetBaseDefinition());
        }
    }
}
