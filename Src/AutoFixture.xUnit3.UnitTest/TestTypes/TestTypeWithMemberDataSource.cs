using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class TestTypeWithMemberDataSource
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "Test is invoked through reflection.")]
        public void SingleStringValueTest(string value)
        {
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.False(string.IsNullOrWhiteSpace(value));
        }

        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "Test is invoked through reflection.")]
        public void MultipleValueTest(string a, int b, decimal c)
        {
            Assert.NotNull(a);
            Assert.NotEmpty(a);
            Assert.False(string.IsNullOrWhiteSpace(a));

            Assert.True(b != default, "Value should not be default");
            Assert.True(c != default, "Value should not be default");
        }

        public void TestWithFrozenParameter(string a, [Frozen] string b, string c)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);

            Assert.NotEqual(a, b);
            Assert.Equal(b, c);
        }

        public static MethodInfo GetSingleStringValueTestMethodInfo() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(SingleStringValueTest));

        public static MethodInfo GetMultipleValueTestMethodInfo() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(MultipleValueTest));

        public static MethodInfo GetTestWithFrozenParameter() =>
            typeof(TestTypeWithMemberDataSource)
                .GetMethod(nameof(TestWithFrozenParameter));
    }
}