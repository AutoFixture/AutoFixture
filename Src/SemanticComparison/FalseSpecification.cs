using System;

namespace Ploeh.SemanticComparison
{
    public class FalseSpecification<T> : ISpecification<T>
    {
        public bool IsSatisfiedBy(T request)
        {
            throw new NotImplementedException();
        }
    }
}
