using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public class TypeWithMethods
    {
        public static void VoidMethodWithString(string arg)
        {
        }

        public static string MethodWithString(string arg)
        {
            return arg;
        }
        public static object[] MethodWithStringArray(object[] arg)
        {
            return arg;
        }
        public static string[] MethodWithStringArray(string[] arg)
        {
            return arg;
        }
        public static T MethodWithGenerics<T>(T arg)
        {
            return arg;
        }
        public static IEnumerable<T> MethodWithEnumerable<T>(IEnumerable<T> arg)
        {
            return arg;
        }
        public static IEnumerable<T> MethodWithArray<T>(T[] arg)
        {
            return arg;
        }
        public static object[] MethodWithOptionalParameter<T>(T arg, string s = "s")
        {
            return new object[] { arg, s };
        }
        public static object[] MethodWithParamsParameter<T>(T arg, params string[] s)
        {
            return new object[] { arg }.Concat(s).ToArray();
        }
        public static object MethodWithSameName(object arg)
        {
            return arg;
        }
        public static string MethodWithSameName(string arg)
        {
            return arg;
        }
    }
}