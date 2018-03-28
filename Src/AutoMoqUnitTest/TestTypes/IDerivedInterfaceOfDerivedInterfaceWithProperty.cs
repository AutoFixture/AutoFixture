namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IDerivedInterfaceOfDerivedInterfaceWithProperty : IDerivedInterfaceWithProperty
    {
        string ThirdProperty { get; set; }
    }
}
