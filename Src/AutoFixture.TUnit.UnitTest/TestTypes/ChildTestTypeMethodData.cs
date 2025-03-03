using System.Reflection;
using System.Threading.Tasks;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    /// <summary>
    /// Created to test whether MemberAutoDataAttribute can discover static test data members from parent classes.
    /// </summary>
    public class ChildTestTypeMethodData : TestTypeWithMethodData
    {
        public new async Task MultipleValueTest(string a, int b, decimal c)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(a).IsNotEmpty();
            await Assert.That(string.IsNullOrWhiteSpace(a)).IsFalse();

            await Assert.That(b != 0, "Value should not be default").IsTrue();
            await Assert.That(c != 0, "Value should not be default").IsTrue();
        }

        public static new MethodInfo GetMultipleValueTestMethodInfo()
        {
            return typeof(ChildTestTypeMethodData)
                .GetMethod(nameof(MultipleValueTest));
        }
    }
}