namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithGetOnlyProperty
    {
        public string GetOnlyProperty
        {
            get { return ""; }
        }
    }
}
