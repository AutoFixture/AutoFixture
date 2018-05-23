namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public interface IInterfaceWithNewMethod : IInterfaceWithShadowedMethod
    {
        // New method.
        new string Method(int i);
    }

    public interface IInterfaceWithShadowedMethod
    {
        // Shadows method.
        string Method(int i);
    }
}
