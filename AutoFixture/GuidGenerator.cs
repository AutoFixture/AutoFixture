using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="Guid"/> instances.
    /// </summary>
    public static class GuidGenerator
    {
        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance.</returns>
        public static Guid CreateAnonymous()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <param name="seed">Ignored.</param>
        /// <returns>A new <see cref="Guid"/> instance.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object CreateAnonymous(object seed)
        {
            return GuidGenerator.CreateAnonymous();
        }
    }
}
