using System.Linq;

namespace AutoFixtureDocumentationTest.Simple
{
    public class MyClass
    {
        public string MyText { get; set; }

        public string DoStuff(string message)
        {
            return new string(message.Reverse().ToArray());
        }

        public T Echo<T>(T item)
        {
            return item;
        }
    }
}
