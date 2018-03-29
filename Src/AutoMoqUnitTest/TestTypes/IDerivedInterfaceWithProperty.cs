namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IDerivedInterfaceWithProperty : IInterfaceWithProperty
    {
        string DerivedProperty { get; set; }
    }
}
