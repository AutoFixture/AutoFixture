using System;
using System.Linq;

namespace Ploeh.VisitReflect
{
    public class CompositeReflectionElement : IReflectionElement
    {
        private readonly IReflectionElement[] elements;

        public CompositeReflectionElement(params IReflectionElement[] elements)
        {
            this.elements = elements;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            return this.elements.Aggregate(visitor, (v, e) => e.Accept(v));
        }
    }
}