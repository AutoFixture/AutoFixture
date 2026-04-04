namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithPrivateField
    {
        private string _field = string.Empty;

        public string GetPrivateField()
        {
            return _field;
        }
    }
}
