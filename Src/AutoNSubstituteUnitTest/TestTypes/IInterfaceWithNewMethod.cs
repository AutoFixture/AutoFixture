namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithNewMethod : IInterfaceWithShadowedMethod
    {
        //new method
        new string Method(int i);
    }
}
