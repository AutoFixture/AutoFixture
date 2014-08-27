namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public class TypeWithPrivateField
    {
        private string field = "";

        public string GetPrivateField()
        {
            return field;
        }
    }
}
