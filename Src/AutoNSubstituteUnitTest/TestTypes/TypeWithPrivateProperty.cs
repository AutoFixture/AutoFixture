namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithPrivateProperty
    {
        protected TypeWithPrivateProperty()
        {
            this.PrivateProperty = "Awesome string";
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private string PrivateProperty { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
