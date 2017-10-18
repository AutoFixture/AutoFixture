namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IInterfaceWithCircularDependency
    {
        IInterfaceWithCircularDependency Component { get; set; }
    }
}