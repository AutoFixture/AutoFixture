using System;

namespace Ploeh.SemanticComparison
{
    public class TrueSpecification<T> : ISpecification<T>
    {
        public bool IsSatisfiedBy(T request)
        {
            return true;
        }
    }
}
