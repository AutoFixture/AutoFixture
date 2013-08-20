namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    public class MyTestClass
    {
        public void OneManualParameter(int a, string b)
        {
        }

        public void TwoStringParameters(string s1, string s2)
        {
        }

        public T Echo<T>(T item)
        {
            return item;
        }
    }
}
