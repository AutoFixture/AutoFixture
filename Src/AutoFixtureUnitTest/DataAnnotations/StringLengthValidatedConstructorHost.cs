using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthValidatedConstructorHost<T>
    {
        public StringLengthValidatedConstructorHost([StringLength(5)] T value)
        {
            this.Value = value;
        }

        public T Value { get; private set; }

        public static ParameterInfo GetParameter()
        {
            return typeof(StringLengthValidatedConstructorHost<T>)
                .GetConstructor(new Type[] { typeof(T) })
                ?.GetParameters().Single();
        }
    }
}