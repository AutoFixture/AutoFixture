namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IInterfaceWithPropertyWithCircularDependency
    {
        IInterfaceWithPropertyWithCircularDependency Property { get; set; }
    }
}
