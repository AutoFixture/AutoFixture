using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    internal class TestTypeWithMemberDataSource
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "Test is invoked through reflection.")]
        public async Task MultipleValueTest(string a, int b, decimal c)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(a).IsNotEmpty();
            await Assert.That(string.IsNullOrWhiteSpace(a)).IsFalse();

            await Assert.That(b != 0, "Value should not be default").IsTrue();
            await Assert.That(c != 0, "Value should not be default").IsTrue();
        }

        public async Task TestWithFrozenParameter(string a, [Frozen] string b, string c)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(b).IsNotNull();
            await Assert.That(c).IsNotNull();

            await Assert.That(b).IsNotEqualTo(a);
            await Assert.That(c).IsEqualTo(b);
        }

        public static MethodInfo GetMultipleValueTestMethodInfo() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(MultipleValueTest));

        public static MethodInfo GetTestWithFrozenParameter() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(TestWithFrozenParameter));
    }
}