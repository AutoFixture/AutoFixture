using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Represents an Equivalence Class for the concept of <i>Many</i>.
    /// </summary>
    public interface IMany
    {
        /// <summary>
        /// Gets or sets the count that specifies how many <i>Many</i> is.
        /// </summary>
        int Count { get; set; }
    }
}
