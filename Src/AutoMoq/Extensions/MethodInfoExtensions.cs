﻿using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoMoq.Extensions
{
    internal static class MethodInfoExtensions
    {
        internal static bool IsOverridable(this MethodInfo method)
        {
            /*
             * From MSDN (http://goo.gl/WvOgYq)
             *
             * To determine if a method is overridable, it is not sufficient to check that IsVirtual is true.
             * For a method to be overridable, IsVirtual must be true and IsFinal must be false.
             *
             * For example, interface implementations are marked as "virtual final".
             * Methods marked with "override sealed" are also marked as "virtual final".
             */

            return method.IsVirtual && !method.IsFinal;
        }

        internal static bool IsSealed(this MethodInfo method)
        {
            return !method.IsOverridable();
        }

        internal static bool IsVoid(this MethodInfo method)
        {
            return method.ReturnType == typeof(void);
        }

        internal static bool HasOutParameters(this MethodInfo method)
        {
            return method.GetParameters()
                         .Any(p => p.IsOut);
        }

        internal static bool HasRefParameters(this MethodInfo method)
        {
            // "out" parameters are also considered "byref", so we have to filter these out
            return method.GetParameters()
                         .Any(p => p.ParameterType.IsByRef && !p.IsOut);
        }
    }
}
