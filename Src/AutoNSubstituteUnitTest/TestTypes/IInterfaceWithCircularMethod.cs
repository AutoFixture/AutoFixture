namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithCircularMethod
    {
        IInterfaceWithCircularMethod Method(object obj);
        object AnotherMethod(object obj);
    }
}
