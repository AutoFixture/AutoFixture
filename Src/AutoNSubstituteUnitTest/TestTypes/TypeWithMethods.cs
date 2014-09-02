using System;
using System.Collections;
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
        public static int MethodInOrder(object arg)
        {
            return 6;
        }
        public static int MethodInOrder(string[] arg1, object arg2 = null)
        {
            return 1;
        }
        public static int MethodInOrder(string[] arg)
        {
            return 0;
        }
        public static int MethodInOrder(IEnumerable<object> arg)
        {
            return 5;
        }
        public static int MethodInOrder(IEnumerable<string> arg)
        {
            return 3;
        }
        public static int MethodInOrder(object[] arg)
        {
            return 4;
        }
        public static int MethodInOrder(string[] arg1, object arg2 = null, params object[] arg3)
        {
            return 2;
        }
        public static int MethodInOrder2(IEnumerable<string> arg1, object arg2)
        {
            return 5;
        }
        public static int MethodInOrder2(string[] arg1, IEnumerable arg2)
        {
            return 3;
        }
        public static int MethodInOrder2(IEnumerable<string> arg1, IEnumerable arg2)
        {
            return 4;
        }
        public static int MethodInOrder2(string[] arg1, IEnumerable<string> arg2, object arg3 = null)
        {
            return 1;
        }
        public static int MethodInOrder2(IEnumerable<string> arg1, IEnumerable<string> arg2)
        {
            return 2;
        }
        public static int MethodInOrder2(string[] arg1, IEnumerable<string> arg2)
        {
            return 0;
        }
        public static int MethodWithFunc<T>(Func<T, T> arg)
        {
            return -1;
        }
        public static bool MethodWithFunc<T>(Func<T> arg)
        {
            return true;
        }
    }
}