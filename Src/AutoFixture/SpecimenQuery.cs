using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Provides convenience methods to perform (partially) anonymous Queries.
    /// </summary>
    public static class SpecimenQuery
    {
        /// <summary>
        /// Invokes the supplied function with an anonymous parameter value and returns the result.
        /// </summary>
        public static TResult Get<T, TResult>(
            this ISpecimenBuilder builder,
            Func<T, TResult> function)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (function == null) throw new ArgumentNullException(nameof(function));

            return function(builder.Create<T>());
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        public static TResult Get<T1, T2, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, TResult> function)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (function == null) throw new ArgumentNullException(nameof(function));

            return function(builder.Create<T1>(), builder.Create<T2>());
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        public static TResult Get<T1, T2, T3, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, T3, TResult> function)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (function == null) throw new ArgumentNullException(nameof(function));

            return function(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>());
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        public static TResult Get<T1, T2, T3, T4, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, T3, T4, TResult> function)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (function == null) throw new ArgumentNullException(nameof(function));

            return function(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>(),
                builder.Create<T4>());
        }
    }
}
