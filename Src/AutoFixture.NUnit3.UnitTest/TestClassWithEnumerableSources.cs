using System.Collections;

namespace AutoFixture.NUnit3.UnitTest
{
    internal class TestClassWithEnumerableSources
    {
        public void TestMethod(int anyInt, double anyDouble, string anyString)
        {
        }

        public static IEnumerable ParameterizedTestCasesMethod(int anyInt, double anyDouble, string anyString)
        {
            yield return new object[] { anyInt, anyDouble, anyString };
            yield return new object[] { anyInt, anyDouble };
            yield return new object[] { anyInt };
        }

        public static IEnumerable TestCasesMethod()
            => ParameterizedTestCasesMethod(4, 3d, "some string");

        public static IEnumerable TestCasesProperty => TestCasesMethod();

        public static IEnumerable TestCasesField = TestCasesMethod();
    }
}