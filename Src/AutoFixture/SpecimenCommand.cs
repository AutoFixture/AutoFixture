using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public static class SpecimenCommand
    {
        public static void Do<T>(
            this ISpecimenBuilder builder,
            Action<T> action)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (action == null)
                throw new ArgumentNullException("action");
            
            action(builder.Create<T>());
        }

        public static void Do<T1, T2>(
            this ISpecimenBuilder builder,
            Action<T1, T2> action)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (action == null)
                throw new ArgumentNullException("action");
            
            action(builder.Create<T1>(), builder.Create<T2>());
        }

        public static void Do<T1, T2, T3>(
            this ISpecimenBuilder builder,
            Action<T1, T2, T3> action)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (action == null)
                throw new ArgumentNullException("action");

            action(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>());
        }

        public static void Do<T1, T2, T3, T4>(
            this ISpecimenBuilder builder,
            Action<T1, T2, T3, T4> action)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (action == null)
                throw new ArgumentNullException("action");

            action(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>(),
                builder.Create<T4>());
        }
    }
}
