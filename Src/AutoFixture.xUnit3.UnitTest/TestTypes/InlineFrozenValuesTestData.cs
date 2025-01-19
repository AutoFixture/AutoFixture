using System;
using System.Collections.Generic;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class InlineFrozenValuesTestData : InlineAttributeTestData
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            // All values provided by fixture
            yield return new object[]
            {
                CreateAttribute(Array.Empty<object>(), ("a", "string_a0"), ("b", "string_b0")),
                TestTypeWithMemberDataSource.GetTestWithFrozenParameter(),
                new object[] { "string_a0", "string_b0", "string_b0" }
            };

            // First parameter injected; Frozen parameter generated
            yield return new object[]
            {
                CreateAttribute(new object[] { "string_a1" }, ("b", "string_b1")),
                TestTypeWithMemberDataSource.GetTestWithFrozenParameter(),
                new object[] { "string_a1", "string_b1", "string_b1" }
            };

            // Frozen parameter is injected
            yield return new object[]
            {
                CreateAttribute(new object[] { "string_a2", "string_b2" }),
                TestTypeWithMemberDataSource.GetTestWithFrozenParameter(),
                new object[] { "string_a2", "string_b2", "string_b2" }
            };
        }
    }
}