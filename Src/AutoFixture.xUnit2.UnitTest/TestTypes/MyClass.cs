namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class MyClass
    {
        public T Echo<T>(T item) => item;
    }
}