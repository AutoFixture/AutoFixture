using System.Collections.Generic;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class TypeWithDataSourceMembers
    {
        public static IEnumerable<string> GetSingleStringTestCases()
        {
            yield return "value-one";
            yield return "value-two";
            yield return "value-three";
        }

        public static IEnumerable<object[]> GetPrimitiveValueTestCases()
        {
            yield return new object[] { "value-one", 12, 23.3m };
            yield return new object[] { "value-two", 38, 12.7m };
            yield return new object[] { "value-three", 94, 52.21m };
        }
    }
}