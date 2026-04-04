namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithGenericParameterMethod
    {
        int GenericMethod<T>(T arg);
    }
}