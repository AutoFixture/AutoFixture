namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public interface IInterfaceWithPropertyWithCircularDependency
    {
        IInterfaceWithPropertyWithCircularDependency Property { get; set; }
    }
}
