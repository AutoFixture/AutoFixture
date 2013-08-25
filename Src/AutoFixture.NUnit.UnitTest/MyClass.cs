namespace Ploe.AutoFixture.NUnit.UnitTest
{
    public class MyClass
    {
        public T Echo<T>(T item)
        {
            return item;
        }
    }
}
