namespace Ploeh.SemanticComparison.UnitTest
{
    public class ResemblanceTest : ProxyTestBase
    {
        protected override TDestination CreateProxy<TSource, TDestination>(Likeness<TSource, TDestination> likeness)
        {
            return likeness.ToResemblance();
        }
    }
}