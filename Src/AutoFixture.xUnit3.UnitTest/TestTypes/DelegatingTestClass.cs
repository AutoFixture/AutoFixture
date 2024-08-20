using System;
using System.Reflection;
using TestTypeFoundation;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class DelegatingTestClass
    {
        public Action<object[]> OnExecute { get; set; }

        public void TestMethod1(int a, decimal b)
        {
            this.OnExecute?.Invoke(new object[] { a, b });
        }

        public void TestMethod2(int a, decimal b, string c, PropertyHolder<float> d)
        {
            this.OnExecute?.Invoke(new object[] { a, b, c, d });
        }

        public static MethodInfo GetTestMethod1()
            => typeof(DelegatingTestClass).GetMethod(nameof(TestMethod1));

        public static MethodInfo GetTestMethod2()
            => typeof(DelegatingTestClass).GetMethod(nameof(TestMethod2));
    }
}