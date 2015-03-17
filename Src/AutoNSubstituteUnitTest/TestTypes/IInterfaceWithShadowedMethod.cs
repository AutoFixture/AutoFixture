namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithShadowedMethod
    {
        //shadowed method
        string Method(int i);
    }
}