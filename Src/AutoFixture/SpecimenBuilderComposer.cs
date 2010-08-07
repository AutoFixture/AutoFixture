using System;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides an API to customize the creational strategy for a specific type.
    /// </summary>
    public static class SpecimenBuilderComposer
    {
        /// <summary>
        /// Invokes the supplied action with an anonymous parameter value.
        /// </summary>
        /// <typeparam name="T">The type of the anonymous parameter.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="action">The action to invoke.</param>
        public static void Do<T>(this ISpecimenBuilderComposer composer, Action<T> action)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            T x = composer.CreateAnonymous<T>();
            action(x);
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="action">The action to invoke.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Action<T1, T2>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Action<T1, T2>, of which it is just a thin wrapper.")]
        public static void Do<T1, T2>(this ISpecimenBuilderComposer composer, Action<T1, T2> action)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            T1 x1 = composer.CreateAnonymous<T1>();
            T2 x2 = composer.CreateAnonymous<T2>();
            action(x1, x2);
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="action">The action to invoke.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Action<T1, T2, T3>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Action<T1, T2, T3>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Action<T1, T2, T3>, of which it is just a thin wrapper.")]
        public static void Do<T1, T2, T3>(this ISpecimenBuilderComposer composer, Action<T1, T2, T3> action)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            T1 x1 = composer.CreateAnonymous<T1>();
            T2 x2 = composer.CreateAnonymous<T2>();
            T3 x3 = composer.CreateAnonymous<T3>();
            action(x1, x2, x3);
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth anonymous parameter.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="action">The action to invoke.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "4", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        public static void Do<T1, T2, T3, T4>(this ISpecimenBuilderComposer composer, Action<T1, T2, T3, T4> action)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            T1 x1 = composer.CreateAnonymous<T1>();
            T2 x2 = composer.CreateAnonymous<T2>();
            T3 x3 = composer.CreateAnonymous<T3>();
            T4 x4 = composer.CreateAnonymous<T4>();
            action(x1, x2, x3, x4);
        }

        /// <summary>
        /// Invokes the supplied function with an anonymous parameter value and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        public static TResult Get<T, TResult>(this ISpecimenBuilderComposer composer, Func<T, TResult> function)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            T x = composer.CreateAnonymous<T>();
            return function(x);
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Func<T1, T2, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Func<T1, T2, TResult>, of which it is just a thin wrapper.")]
        public static TResult Get<T1, T2, TResult>(this ISpecimenBuilderComposer composer, Func<T1, T2, TResult> function)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            T1 x1 = composer.CreateAnonymous<T1>();
            T2 x2 = composer.CreateAnonymous<T2>();
            return function(x1, x2);
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Func<T1, T2, T3, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Func<T1, T2, T3, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Func<T1, T2, T3, TResult>, of which it is just a thin wrapper.")]
        public static TResult Get<T1, T2, T3, TResult>(this ISpecimenBuilderComposer composer, Func<T1, T2, T3, TResult> function)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            T1 x1 = composer.CreateAnonymous<T1>();
            T2 x2 = composer.CreateAnonymous<T2>();
            T3 x3 = composer.CreateAnonymous<T3>();
            return function(x1, x2, x3);
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "4", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        public static TResult Get<T1, T2, T3, T4, TResult>(this ISpecimenBuilderComposer composer, Func<T1, T2, T3, T4, TResult> function)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            T1 x1 = composer.CreateAnonymous<T1>();
            T2 x2 = composer.CreateAnonymous<T2>();
            T3 x3 = composer.CreateAnonymous<T3>();
            T4 x4 = composer.CreateAnonymous<T4>();
            return function(x1, x2, x3, x4);
        }
    }
}
