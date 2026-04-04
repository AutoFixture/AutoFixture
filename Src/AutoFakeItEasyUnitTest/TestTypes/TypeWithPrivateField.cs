namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public class TypeWithPrivateField
    {
        private string _field = string.Empty;

        public string GetPrivateField()
        {
            return _field;
        }
    }
}
