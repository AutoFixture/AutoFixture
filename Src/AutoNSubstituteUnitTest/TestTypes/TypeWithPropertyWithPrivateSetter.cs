namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithPropertyWithPrivateSetter
    {
        protected TypeWithPropertyWithPrivateSetter()
        {
            PropertyWithPrivateSetter = "Awesome string";
        }

        public string PropertyWithPrivateSetter { get; private set; }
    }
}
