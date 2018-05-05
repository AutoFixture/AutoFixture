namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public interface IInterfaceWithRefMethod
    {
        string Method(ref string s);
        int Method(ref int i);
    }
}
