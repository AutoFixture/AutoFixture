using System;

namespace Ploeh.AutoFixture
{
    public static class CompatibilityExtensions
    {
        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="fixture">
        /// The fixture used to resolve the type request.
        /// </param>
        /// <returns>
        /// An anonymous object of type <typeparamref name="T"/>.
        /// </returns>
        [Obsolete("For compatibility to AutoFixture version 2. This method will be removed, please move to using Create<T>()")]
        public static T CreateAnonymous<T>(this IFixture fixture)
        {
            return fixture.Create<T>();
        }
    }
}