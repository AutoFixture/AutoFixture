using System;

namespace TestTypeFoundation
{
    public static class GuardedStaticMethodOnStaticTypeHost
    {
        public static void Method(object argument)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));
        }
    }
}