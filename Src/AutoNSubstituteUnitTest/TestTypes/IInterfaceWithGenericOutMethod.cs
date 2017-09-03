namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithGenericOutMethod
    {
        int GenericMethod<T>(out T retValue);
    }
}