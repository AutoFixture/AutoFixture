namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithGenericMethod
    {
        T GenericMethod<T>();
    }
}
