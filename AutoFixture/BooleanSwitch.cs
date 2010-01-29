using System.ComponentModel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates an alternating sequence of <see langword="true"/> and <see langword="false"/>,
    /// </summary>
    public class BooleanSwitch
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
        public bool CreateAnonymous()
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object CreateAnonymous(object seed)
        {
            return this.CreateAnonymous();
        }
    }
}
