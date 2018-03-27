namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IDerivedInterfaceWithProperty : IInterfaceWithProperty
    {
        string SecondProperty { get; set; }
    }
}
