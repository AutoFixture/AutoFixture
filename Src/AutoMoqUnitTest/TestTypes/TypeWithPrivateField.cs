namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithPrivateField
    {
        private string field = "";

        public string GetPrivateField()
        {
            return this.field;
        }
    }
}
