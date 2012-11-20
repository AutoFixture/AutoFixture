using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides convenience methods to perform (partially) anonymous Queries.
    /// </summary>
    public static class SpecimenQuery
    {
        /// <summary>
        /// Invokes the supplied function with an anonymous parameter value and
        /// returns the result.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the anonymous parameter.
        /// </typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// function is null
        /// </exception>
        public static TResult Get<T, TResult>(
            this ISpecimenBuilder builder,
            Func<T, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(builder.Create<T>());
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and
        /// returns the result.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the first anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The type of the second anonymous parameter.
        /// </typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// function is null
        /// </exception>
        public static TResult Get<T1, T2, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(builder.Create<T1>(), builder.Create<T2>());
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and
        /// returns the result.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the first anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The type of the second anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The type of the third anonymous parameter.
        /// </typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// function is null
        /// </exception>
        public static TResult Get<T1, T2, T3, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, T3, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>());
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and
        /// returns the result.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the first anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The type of the second anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The type of the third anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T4">
        /// The type of the fourth anonymous parameter.
        /// </typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// function is null
        /// </exception>
        public static TResult Get<T1, T2, T3, T4, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, T3, T4, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>(),
                builder.Create<T4>());
        }
    }
}
