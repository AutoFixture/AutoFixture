using System;
using System.Reflection;

namespace TestTypeFoundation
{
    public class TypeWithOverloadedMembers
    {
        public object SomeProperty { get; set; }

        public void DoSomething()
        {
        }

        public void DoSomething(object obj)
        {
        }

        public void DoSomething(object x, object y)
        {
        }

        public void DoSomething(object x, object y, object z)
        {
        }

        public static MethodInfo GetDoSomethingMethod(params Type[] parameterTypes) =>
            typeof(TypeWithOverloadedMembers)
                .GetMethod(nameof(DoSomething), parameterTypes);
    }
}
