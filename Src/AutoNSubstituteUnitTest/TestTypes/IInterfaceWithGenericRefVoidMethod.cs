namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithGenericRefVoidMethod
    {
        void GenericMethod<T>(ref T retValue);
    }
}