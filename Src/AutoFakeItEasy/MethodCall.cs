using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.AutoFakeItEasy
{
    internal class MethodCall
    {
        private readonly string methodName;
        private readonly IList<Type> parameterTypes;
        private readonly IList<object> arguments;

        public MethodCall(string methodName, IEnumerable<Type> parameterTypes, IEnumerable<object> arguments)
        {
            this.methodName = methodName;
            this.parameterTypes = parameterTypes.ToList();
            this.arguments = arguments.ToList();
        }

        public override bool Equals(object obj)
        {
            return obj is MethodCall call &&
                   this.methodName.Equals(call.methodName, StringComparison.Ordinal) &&
                   this.parameterTypes.SequenceEqual(call.parameterTypes) &&
                   this.arguments.SequenceEqual(call.arguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -712421553;
                hashCode = hashCode * -1521134295 + this.methodName.GetHashCode();
                foreach (var argument in this.arguments)
                {
                    hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(argument);
                }
                foreach (var argumentType in this.parameterTypes)
                {
                    hashCode = hashCode * -1521134295 + argumentType.GetHashCode();
                }
                return hashCode;
            }
        }
    }
}