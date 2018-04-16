namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public interface IDerivedInterfaceWithProperty : IInterfaceWithProperty
    {
        string DerivedProperty { get; set; }
    }
}
