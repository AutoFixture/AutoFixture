using System.Reflection;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    /// <summary>
    /// Created to test whether MemberAutoDataAttribute can discover static test data members from parent classes.
    /// </summary>
    public class ChildTestTypeMethodData : TestTypeWithMethodData
    {
        public new void MultipleValueTest(string a, int b, decimal c)
        {
            Assert.NotNull(a);
            Assert.NotEmpty(a);
            Assert.False(string.IsNullOrWhiteSpace(a));

            Assert.True(b != 0, "Value should not be default");
            Assert.True(c != 0, "Value should not be default");
        }

        public static new MethodInfo GetMultipleValueTestMethodInfo()
        {
            return typeof(ChildTestTypeMethodData)
                .GetMethod(nameof(MultipleValueTest));
        }
    }
}