using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Defines overloaded operations on any <see cref="IMemberContext"/> instance.
    /// </summary>
    public static class MemberContext
    {
        /// <summary>
        /// Verifies the boundaries conditions of the type member(s) encapsulated by the context,
        /// using a default <see cref="IBoundaryConvention"/>.
        /// </summary>
        /// <param name="memberContext">The context that encapsulates one or more members.</param>
        public static void VerifyBoundaries(this IMemberContext memberContext)
        {
            if (memberContext == null)
            {
                throw new ArgumentNullException("memberContext");
            }

            memberContext.VerifyBoundaries(new DefaultBoundaryConvention());
        }
    }
}
