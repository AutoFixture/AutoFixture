using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.Albedo;

namespace Ploeh.AutoFixture.Idioms
{
    internal class SemanticElementComparer : IEqualityComparer<IReflectionElement>
    {
        private readonly IReflectionVisitor<IEnumerable> visitor;

        internal SemanticElementComparer(
            IReflectionVisitor<IEnumerable> visitor)
        {
            this.visitor = visitor;
        }

        public bool Equals(IReflectionElement x, IReflectionElement y)
        {
            var values = new CompositeReflectionElement(x, y)
                .Accept(this.visitor)
                .Value
                .Cast<object>()
                .ToArray();
            return values.Length == 2
                   && values.Distinct().Count() == 1;
        }

        public int GetHashCode(IReflectionElement obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return obj
                .Accept(this.visitor)
                .Value
                .Cast<object>()
                .Single()
                .GetHashCode();
        }
    }
}