namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithRefMethod
    {
        string Method(ref string s);
    }
}
