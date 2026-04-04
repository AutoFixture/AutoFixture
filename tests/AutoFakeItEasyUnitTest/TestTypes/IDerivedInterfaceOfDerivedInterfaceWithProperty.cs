namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public interface IDerivedInterfaceOfDerivedInterfaceWithProperty : IDerivedInterfaceWithProperty
    {
        string DerivedDerivedProperty { get; set; }
    }
}
