using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class SByteSequenceGenerator
    {
        private sbyte s;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SByteSequenceGenerator"/> class.
        /// </summary>
        public SByteSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        [CLSCompliant(false)]
        public sbyte CreateAnonymous()
        {
            lock (this.syncRoot)
            {
                return ++this.s;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consequtive sequence.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object CreateAnonymous(object seed)
        {
            return this.CreateAnonymous();
        }
    }
}
