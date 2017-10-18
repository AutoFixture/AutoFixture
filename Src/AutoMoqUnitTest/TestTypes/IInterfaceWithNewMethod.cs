namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IInterfaceWithShadowedMethod
    {
        //shadowed method
        string Method(int i);
    }

    public interface IInterfaceWithNewMethod : IInterfaceWithShadowedMethod
    {
        //new method
        new string Method(int i);
    }
}
