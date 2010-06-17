using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class SByteSequenceGenerator : ISpecimenBuilder
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

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates an anonymous <see cref="SByte"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">Not used.</param>
        /// <returns>
        /// The next <see cref="SByte"/> in a consequtive sequence, if <paramref name="request"/>
        /// is a request for an SByte; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (request != typeof(sbyte))
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous();
        }

        #endregion
    }
}
