using System;
using System.Collections.Generic;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    internal class InlineFrozenValuesTestData : InlineAttributeTestData
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            // All values provided by fixture
            yield return
            [
                CreateAttribute([], ("a", "string_a0"), ("b", "string_b0")),
                TestTypeWithMemberDataSource.GetTestWithFrozenParameter(),
                new object[] { "string_a0", "string_b0", "string_b0" }
            ];

            // First parameter injected; Frozen parameter generated
            yield return
            [
                CreateAttribute(["string_a1"], ("b", "string_b1")),
                TestTypeWithMemberDataSource.GetTestWithFrozenParameter(),
                new object[] { "string_a1", "string_b1", "string_b1" }
            ];

            // Frozen parameter is injected
            yield return
            [
                CreateAttribute(["string_a2", "string_b2"]),
                TestTypeWithMemberDataSource.GetTestWithFrozenParameter(),
                new object[] { "string_a2", "string_b2", "string_b2" }
            ];
        }
    }
}