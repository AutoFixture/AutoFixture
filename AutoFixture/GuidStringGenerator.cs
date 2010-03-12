using System;
using System.Globalization;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates strings based on an optional suffix string and <see cref="Guid"/>.
    /// </summary>
    public class GuidStringGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an anonymous string by converting a new <see cref="Guid"/> to a string and
        /// prefixing it with an optional name.
        /// </summary>
        /// <param name="name">
        /// An optional name that will be prefixed to the generated <see cref="Guid"/> string, if
        /// supplied.
        /// </param>
        /// <returns>
        /// A string consisting of <paramref name="name"/> concatenated with a <see cref="Guid"/>
        /// string.
        /// </returns>
        public static string CreateAnonymous(string name)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", name, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Creates an anonymous string by converting a new <see cref="Guid"/> to a string and
        /// prefixing it wiht an optional name.
        /// </summary>
        /// <param name="seed">
        /// An optional name that will be prefixed to the generated <see cref="Guid"/> string, if
        /// supplied. Must be a string.
        /// </param>
        /// <returns>
        /// A string consisting of <paramref name="seed"/> concatenated with a <see cref="Guid"/>
        /// string.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object CreateAnonymous(object seed)
        {
            return GuidStringGenerator.CreateAnonymous((string)seed);
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new string from a <see cref="Guid"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">Not used.</param>
        /// <returns>
        /// A new string created from a new <see cref="Guid"/> if <paramref name="request"/>
        /// represents a string; otherwise, <see langword="null"/>.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (request != typeof(string))
            {
                return new NoSpecimen(request);
            }

            return Guid.NewGuid().ToString();
        }

        #endregion
    }
}
