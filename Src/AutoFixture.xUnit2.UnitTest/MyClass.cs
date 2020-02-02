namespace AutoFixture.Xunit2.UnitTest
{
    public class MyClass
    {
        public string Prop1 { get; set; }

        public T Echo<T>(T item)
        {
            return item;
        }
    }
}