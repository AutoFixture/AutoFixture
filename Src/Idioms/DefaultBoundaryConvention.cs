using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a default set of conventions that describes expected API boundary behavior.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Examples of expected behavior are that all reference type parameters are guarded against
    /// null references, <see cref="Guid"/> arguments are guarded against <see cref="Guid.Empty"/>,
    /// etc.
    /// </para>
    /// </remarks>
    public class DefaultBoundaryConvention : CompositeBoundaryConvention
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBoundaryConvention"/> class.
        /// </summary>
        public DefaultBoundaryConvention()
            : base(
                new GuidBoundaryConvention(),
                new ReferenceTypeBoundaryConvention())
        {
        }
    }
}
