using System;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Defines an expectation about a behavior at a given API boundary.
    /// </summary>
    public interface IBoundaryBehavior
    {
        /// <summary>
        /// Asserts that the specified action behaves correctly.
        /// </summary>
        /// <param name="action">The action to verify.</param>
        /// <param name="context">
        /// A text describing the context of <paramref name="action"/>.
        /// </param>
        void Assert(Action<object> action, string context);
    }
}
