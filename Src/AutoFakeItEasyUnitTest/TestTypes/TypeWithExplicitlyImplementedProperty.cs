namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public class TypeWithExplicitlyImplementedProperty: IInterfaceWithProperty
    {
        string IInterfaceWithProperty.Property { get; set; }
    }
}
