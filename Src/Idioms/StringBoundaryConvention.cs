using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a convention for API boundary behavior that applies to <see cref="string"/>
    /// instances.
    /// </summary>
    public class StringBoundaryConvention : IBoundaryConvention
    {
        #region Implementation of IBoundaryConvention

        /// <summary>
        /// Creates <see cref="IBoundaryBehavior"/> instances for a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A sequence of <see cref="IBoundaryBehavior"/> instances that match
        /// <paramref name="type"/>.
        /// </returns>
        public IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type == typeof(string))
            {
                yield return new EmptyStringBehavior();
            }
        }

        #endregion
    }
}