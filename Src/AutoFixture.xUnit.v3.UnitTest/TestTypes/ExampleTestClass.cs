using System;
using System.Diagnostics.CodeAnalysis;
using TestTypeFoundation;

namespace AutoFixture.Xunit.v3.UnitTest.TestTypes
{
    public class ExampleTestClass
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "This test method is used through reflection.")]
        public void TestMethod(int a, string b, EnumType c, Tuple<string, int> d)
        {
        }
    }
}