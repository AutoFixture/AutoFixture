using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Expresses idiomatic assertions related to one or more properties.
    /// </summary>
    public interface IPropertyContext : IMemberContext
    {
        /// <summary>
        /// Verifies that the property or properties encapsulated by the context is or are
        /// writable, and that the value returned is the same as the value which was originally
        /// assigned.
        /// </summary>
        void VerifyWritable();
    }
}
