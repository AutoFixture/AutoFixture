using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Provides convenience methods to perform (partially) anonymous Commands.
    /// </summary>
    public static class SpecimenCommand
    {
        /// <summary>
        /// Invokes the supplied action with an anonymous parameter value.
        /// </summary>
        public static void Do<T>(
            this ISpecimenBuilder builder,
            Action<T> action)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(builder.Create<T>());
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        public static void Do<T1, T2>(
            this ISpecimenBuilder builder,
            Action<T1, T2> action)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(builder.Create<T1>(), builder.Create<T2>());
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        public static void Do<T1, T2, T3>(
            this ISpecimenBuilder builder,
            Action<T1, T2, T3> action)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>());
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        public static void Do<T1, T2, T3, T4>(
            this ISpecimenBuilder builder,
            Action<T1, T2, T3, T4> action)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>(),
                builder.Create<T4>());
        }
    }
}
