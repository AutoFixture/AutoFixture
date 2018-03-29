namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public interface IDerivedInterfaceOfDerivedInterfaceWithProperty : IDerivedInterfaceWithProperty
    {
        string DerivedDerivedProperty { get; set; }
    }
}
