namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithGenericOutVoidMethod
    {
        void GenericMethod<T>(out T retValue);
    }
}