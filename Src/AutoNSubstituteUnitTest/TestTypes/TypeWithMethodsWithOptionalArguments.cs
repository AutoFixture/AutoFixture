using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithMethodsWithOptionalArguments
    {
        private TypeWithMethodsWithOptionalArguments()
        {
        }

        public static string[] MethodWithOptionalArgumentString(string argument, string optional = "100")
        {
            return new[] { argument, optional };
        }

        public static int[] MethodWithOptionalArgument(int argument, int optional = 100)
        {
            return new[] { argument, optional };
        }

        public static string[] MethodWithParamsArgumentString(string argument, params string[] arguments)
        {
            return new[] { argument }.Concat(arguments).ToArray();
        }

        public static int[] MethodWithParamsArgument(int argument, params int[] arguments)
        {
            return new[] { argument }.Concat(arguments).ToArray();
        }

        public static int[] MethodWithOptionalAndParamsArguments(int argument, int optional = 200, params int[] arguments)
        {
            return new[] { argument, optional }.Concat(arguments).ToArray();
        }

        public static string[] MethodWithOptionalAndParamsArgumentsString(string argument, string optional = "200", params string[] arguments)
        {
            return new[] { argument, optional }.Concat(arguments).ToArray();
        }
    }
}