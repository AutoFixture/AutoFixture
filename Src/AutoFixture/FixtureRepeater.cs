using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture
{
    /// <summary>
    /// Contains extension methods for repeating a function in <see cref="IFixture"/> instances.
    /// </summary>
    public static class FixtureRepeater
    {
        /// <summary>
        /// Repeats a function many times.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object that <paramref name="function"/> creates.
        /// </typeparam>
        /// <param name="fixture">
        /// The <see cref="IFixture"/>to use.
        /// </param>
        /// <param name="function">
        /// A function that creates an instance of <typeparamref name="T"/>.
        /// </param>
        /// <returns>A sequence of objects created by <paramref name="function"/>.</returns>
        /// <remarks>
        /// <para>
        /// The number of times <paramref name="function"/> is invoked is determined by
        /// <see cref="IFixture.RepeatCount"/>.
        /// </para>
        /// </remarks>
        public static IEnumerable<T> Repeat<T>(this IFixture fixture, Func<T> function)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            return from f in Enumerable.Repeat(function, fixture.RepeatCount)
                   select f();
        }
    }
}