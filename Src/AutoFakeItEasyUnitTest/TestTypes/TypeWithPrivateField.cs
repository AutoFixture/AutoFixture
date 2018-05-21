namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public class TypeWithPrivateField
    {
        private string field = string.Empty;

        public string GetPrivateField()
        {
            return this.field;
        }
    }
}
