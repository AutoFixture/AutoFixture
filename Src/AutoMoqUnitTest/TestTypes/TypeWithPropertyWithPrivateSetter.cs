namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithPropertyWithPrivateSetter
    {
        public TypeWithPropertyWithPrivateSetter()
        {
            PropertyWithPrivateSetter = "Awesome string";
        }

        public string PropertyWithPrivateSetter { get; private set; }
    }
}
