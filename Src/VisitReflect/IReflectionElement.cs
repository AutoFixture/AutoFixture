using System;

namespace Ploeh.VisitReflect
{
    /// <summary>
    /// Represents a polymorphic reflection element, which can be visited
    /// by the <see cref="IReflectionVisitor{T}"/>.
    /// </summary>
    public interface IReflectionElement
    {
        /// <summary>
        /// Accepts the <see cref="IReflectionVisitor{T}"/> as per the 
        /// visitor pattern http://en.wikipedia.org/wiki/Visitor_pattern
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of element being visited</typeparam>
        /// <param name="visitor">The visitor to accept</param>
        /// <returns></returns>
        IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor);
    }
}