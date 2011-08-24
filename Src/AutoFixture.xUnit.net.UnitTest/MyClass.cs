namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class MyClass
    {
        public T Echo<T>(T item)
        {
            return item;
        }
    }
}
