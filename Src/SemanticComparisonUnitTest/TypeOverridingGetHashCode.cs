namespace Ploeh.SemanticComparison.UnitTest
{
    public class TypeOverridingGetHashCode
    {
        public override int GetHashCode()
        {
            return 14;
        }
    }
}
