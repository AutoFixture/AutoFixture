using System;
using System.Collections.Generic;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class InlinePrimitiveValuesTestData : InlineAttributeTestData
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            // All values provided by fixture
            yield return new object[]
            {
                CreateAttributeWithFakeFixture(Array.Empty<object>(), ("a", "parameter_a1"), ("b", 31), ("c", 54.38m)),
                TestTypeWithMemberDataSource.GetMultipleValueTestMethodInfo(),
                new object[] { "parameter_a1", 31, 54.38m }
            };

            // Some parameters injected, some provided by fixture
            yield return new object[]
            {
                CreateAttributeWithFakeFixture(new object[] { "parameter_a2" }, ("b", 22), ("c", 817.218m)),
                TestTypeWithMemberDataSource.GetMultipleValueTestMethodInfo(),
                new object[] { "parameter_a2", 22, 817.218m }
            };

            // All parameters injected
            yield return new object[]
            {
                CreateAttributeWithFakeFixture(new object[] { "parameter_a3", 13, 332.009m }),
                TestTypeWithMemberDataSource.GetMultipleValueTestMethodInfo(),
                new object[] { "parameter_a3", 13, 332.009m }
            };
        }
    }
}