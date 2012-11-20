using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides convenience methods to perform (partially) anonymous Commands.
    /// </summary>
    public static class SpecimenCommand
    {
        /// <summary>
        /// Invokes the supplied action with an anonymous parameter value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the anonymous parameter.
        /// </typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="action">The action to invoke.</param>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// action is null
        /// </exception>
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

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the first anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The type of the second anonymous parameter.
        /// </typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="action">The action to invoke.</param>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// action is null
        /// </exception>
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

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// /// <typeparam name="T1">
        /// The type of the first anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The type of the second anonymous parameter.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The type of the third anonymous parameter.
        /// </typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="action">The action to invoke.</param>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// action is null
        /// </exception>
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

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
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
        /// <param name="builder">The builder.</param>
        /// <param name="action">The action to invoke.</param>
        /// <exception cref="System.ArgumentNullException">
        /// builder is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// action is null
        /// </exception>
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
