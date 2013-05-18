namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeOverridingGetHashCode
    {
        public override int GetHashCode()
        {
            return 14;
        }
    }
}
