namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithGenericRefMethod
    {
        int GenericMethod<T>(ref T retValue);
    }
}