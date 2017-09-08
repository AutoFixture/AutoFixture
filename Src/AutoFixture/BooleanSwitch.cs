using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates an alternating sequence of <see langword="true"/> and <see langword="false"/>,
    /// </summary>
    public class BooleanSwitch : ISpecimenBuilder
    {
        private bool b;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanSwitch"/> class.
        /// </summary>
        public BooleanSwitch()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Returns an alternating sequence of <see langword="true"/> and <see langword="false"/>
        /// every other time it is invoked.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, followed by <see langword="false"/> at the next invocation, and
        /// so on.
        /// </returns>
        public bool Create()
        {
            lock (this.syncRoot)
            {
                this.b = !this.b;
                return this.b;
            }
        }

        /// <summary>
        /// Returns an alternating sequence of <see langword="true"/> and <see langword="false"/>
        /// every other time it is invoked.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, followed by <see langword="false"/> at the next invocation, and
        /// so on.
        /// </returns>
        /// <remarks>Obsolete: Please move over to using <see cref="Create()">Create()</see> as this method will be removed in the next release</remarks>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release")]
        public bool CreateAnonymous()
        {
            return Create();
        }

        /// <summary>
        /// Returns an alternating sequence of <see langword="true"/> and <see langword="false"/>
        /// every other time it is invoked.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// <see langword="true"/>, followed by <see langword="false"/> at the next invocation, and
        /// so on, if <paramref name="request"/> is a request for a boolean; otherwise, a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(bool).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.Create();
        }
    }
}
