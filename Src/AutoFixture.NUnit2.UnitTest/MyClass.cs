namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    public class MyClass
    {
        public T Echo<T>(T item)
        {
            return item;
        }
    }
}
