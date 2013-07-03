namespace Ploeh.SemanticComparison
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T request);
    }
}