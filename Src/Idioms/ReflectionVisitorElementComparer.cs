using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.Albedo;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Allows comparing <see cref="IReflectionElement"/> instances, where the comparison
    /// is implemented by collecting the elements using an <see cref="IReflectionVisitor{T}"/>
    /// then comparing them using
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class ReflectionVisitorElementComparer<T> : IEqualityComparer<IReflectionElement>
    {
        private readonly IReflectionVisitor<IEnumerable<T>> visitor;
        private readonly IEqualityComparer<T> comparer;

        internal ReflectionVisitorElementComparer(
            IReflectionVisitor<IEnumerable<T>> visitor,
            IEqualityComparer<T> comparer = null)
        {
            this.visitor = visitor;
            this.comparer = comparer ?? EqualityComparer<T>.Default;
        }

        bool IEqualityComparer<IReflectionElement>.Equals(IReflectionElement x, IReflectionElement y)
        {
            var values = new CompositeReflectionElement(x, y)
                .Accept(this.visitor)
                .Value
                .ToArray();

            var distinctValues = values.Distinct(this.comparer);

            return values.Length == 2
                   && distinctValues.Count() == 1;
        }

        int IEqualityComparer<IReflectionElement>.GetHashCode(IReflectionElement obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return obj
                .Accept(this.visitor)
                .Value
                .Single()
                .GetHashCode();
        }
    }
}