using System;
using System.Diagnostics.CodeAnalysis;
using TestTypeFoundation;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class ExampleTestClass
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "This test method is used through reflection.")]
        public void TestMethod(int a, string b, EnumType c, Tuple<string, int> d)
        {
        }
    }

    public class ExampleTestClass<T1, T2, T3, T4>
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "This test method is used through reflection.")]
        public void TestMethod(T1 a, T2 b, T3 c, T4 d)
        {
        }
    }
}
