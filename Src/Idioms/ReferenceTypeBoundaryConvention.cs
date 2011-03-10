using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a general convention for API boundary behavior that applies to all reference
    /// types.
    /// </summary>
    public class ReferenceTypeBoundaryConvention : IBoundaryConvention
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

            if (type.IsClass
                || type.IsInterface)
            {
                yield return new NullReferenceBehavior();
            }
        }

        #endregion
    }
}