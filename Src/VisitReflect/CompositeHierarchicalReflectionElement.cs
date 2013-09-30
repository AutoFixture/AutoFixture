using System;
using System.Linq;

namespace Ploeh.VisitReflect
{
    public class CompositeHierarchicalReflectionElement : IHierarchicalReflectionElement
    {
        private readonly IHierarchicalReflectionElement[] elements;

        public CompositeHierarchicalReflectionElement(params IHierarchicalReflectionElement[] elements)
        {
            this.elements = elements;
        }
        
        public IHierarchicalReflectionVisitor<T> Accept<T>(IHierarchicalReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            return this.elements.Aggregate(visitor, (v, e) => e.Accept(v));
        }
    }
}