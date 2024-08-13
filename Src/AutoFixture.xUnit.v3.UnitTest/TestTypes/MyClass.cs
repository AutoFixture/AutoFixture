namespace AutoFixture.Xunit.v3.UnitTest.TestTypes
{
    public class MyClass
    {
        public T Echo<T>(T item) => item;
    }
}