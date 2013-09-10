namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    internal class MyTestClass
    {
        public void TwoStringParameters(string s1, string s2)
        {
        }

        public T Echo<T>(T item)
        {
            return item;
        }
    }
}
