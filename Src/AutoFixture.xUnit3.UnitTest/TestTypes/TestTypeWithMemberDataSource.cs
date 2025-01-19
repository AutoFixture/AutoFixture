using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class TestTypeWithMemberDataSource
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "Test is invoked through reflection.")]
        public void MultipleValueTest(string a, int b, decimal c)
        {
            Assert.NotNull(a);
            Assert.NotEmpty(a);
            Assert.False(string.IsNullOrWhiteSpace(a));

            Assert.True(b != 0, "Value should not be default");
            Assert.True(c != 0, "Value should not be default");
        }

        public void TestWithFrozenParameter(string a, [Frozen] string b, string c)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);

            Assert.NotEqual(a, b);
            Assert.Equal(b, c);
        }

        public static MethodInfo GetMultipleValueTestMethodInfo() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(MultipleValueTest));

        public static MethodInfo GetTestWithFrozenParameter() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(TestWithFrozenParameter));
    }
}