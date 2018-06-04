namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithPrivateField
    {
        private string field = string.Empty;

        public string GetPrivateField()
        {
            return this.field;
        }
    }
}
