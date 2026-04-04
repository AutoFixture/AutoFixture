namespace AutoFixture.AutoFakeItEasy.UnitTest.TestTypes
{
    public interface IInterfaceWithParamsMethod
    {
        int Method(params int[] values);
        int this[params int[] indexes] { get; set; }
    }
}