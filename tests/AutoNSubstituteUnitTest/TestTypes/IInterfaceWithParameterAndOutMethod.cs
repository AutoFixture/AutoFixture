namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithParameterAndOutMethod
    {
        bool Method(string parameter, out int i);
    }
}
