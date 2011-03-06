using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Expresses idiomatic assertions related to one or more methods.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "This interface mainly exists to provide a degree of symmetry to the IPropertyContext interface. In the future, members may be added to this interface.")]
    public interface IMethodContext : IMemberContext
    {
    }
}
