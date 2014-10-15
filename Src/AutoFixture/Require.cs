using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains helper methods that check various kinds of preconditions.
    /// </summary>
    internal static class Require
    {
        /// <summary>
        /// Checks that the specified argument is not null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="name">The name of the argument to put in the exception message.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the specified <paramref name="argument"/> is null.
        /// </exception>
        internal static void IsNotNull(object argument, string name = null)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
