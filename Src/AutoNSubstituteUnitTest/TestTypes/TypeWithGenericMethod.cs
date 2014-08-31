using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithGenericMethod
    {
        private TypeWithGenericMethod() 
        {
        }

        public static TypeWithGenericMethod Create<T>(T argument)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T>(IEnumerable<T> arguments)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T, U>(T argument1, U argument2)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T, U>(T argument1, Func<T, U> argument2)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T>(T[] argument1, string[] argument2)
        {
            return new TypeWithGenericMethod();
        }

        public static T Create<T>()
        {
            return default(T);
        }
    }
}