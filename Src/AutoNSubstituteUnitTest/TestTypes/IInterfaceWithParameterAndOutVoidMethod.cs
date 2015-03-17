namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithParameterAndOutVoidMethod
    {
        void Method(string parameter, out int i);
    }
}
