namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithPrivateProperty
    {
        public TypeWithPrivateProperty()
        {
            PrivateProperty = "Awesome string";
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private string PrivateProperty { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
