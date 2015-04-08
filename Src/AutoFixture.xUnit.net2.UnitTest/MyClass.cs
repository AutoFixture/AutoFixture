namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    public class MyClass
    {
        public T Echo<T>(T item)
        {
            return item;
        }
    }
}