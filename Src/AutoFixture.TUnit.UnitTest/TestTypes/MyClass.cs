namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class MyClass
    {
        public T Echo<T>(T item) => item;
    }
}