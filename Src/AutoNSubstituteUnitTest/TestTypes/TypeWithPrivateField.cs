namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithPrivateField
    {
        private string field = "";

        public string GetPrivateField()
        {
            return this.field;
        }
    }
}
