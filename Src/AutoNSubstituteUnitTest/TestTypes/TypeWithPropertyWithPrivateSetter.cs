namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithPropertyWithPrivateSetter
    {
        protected TypeWithPropertyWithPrivateSetter()
        {
            this.PropertyWithPrivateSetter = "Awesome string";
        }

        public string PropertyWithPrivateSetter { get; private set; }
    }
}
