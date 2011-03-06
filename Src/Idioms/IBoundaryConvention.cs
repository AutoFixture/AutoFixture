using System.Collections.Generic;
using System;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Defines a set of <see cref="IBoundaryBehavior"/> instances. Together, they form a
    /// convention that describes how an API should behave in boundary cases.
    /// </summary>
    public interface IBoundaryConvention
    {
        /// <summary>
        /// Creates <see cref="IBoundaryBehavior"/> instances for a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A sequence of <see cref="IBoundaryBehavior"/> instances that match
        /// <paramref name="type"/>.
        /// </returns>
        IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Type type);
    }
}