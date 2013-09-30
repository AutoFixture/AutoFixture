using System;

namespace Ploeh.VisitReflect
{
    /// <summary>
    /// Represents a polymorphic reflection element, which can be visited
    /// by an <see cref="IReflectionVisitor{T}"/> implementation.
    /// </summary>
    public interface IReflectionElement
    {
        /// <summary>
        /// Accepts the <see cref="IReflectionVisitor{T}"/> as per the 
        /// visitor pattern http://en.wikipedia.org/wiki/Visitor_pattern
        /// </summary>
        /// <typeparam name="T">The type of observation(s) of the vistor</typeparam>
        /// <param name="visitor">The visitor to accept</param>
        /// <returns></returns>
        IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor);
    }
}