using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Expresses idiomatic assertions related to one or more type members.
    /// </summary>
    public interface IMemberContext
    {
        /// <summary>
        /// Verifies the boundaries conditions of the type member(s) encapsulated by the context.
        /// </summary>
        /// <param name="convention">The convention to use to verify the boundaries.</param>
        /// <remarks>
        /// <para>
        /// An example of a convention could be to verify that all method parameters are protected
        /// by Guard Clauses the protect against null references.
        /// </para>
        /// </remarks>
        void VerifyBoundaries(IBoundaryConvention convention);
    }
}
