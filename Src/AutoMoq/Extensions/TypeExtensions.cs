using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.Extensions
{
    internal static class TypeExtensions
    {
        public static IEnumerable<MethodInfo> GetInterfaceMethods(this Type type)
        {
            var allMethods = type.GetMethods().Concat(
                type.GetInterfaces()
                    .SelectMany(@interface => @interface.GetMethods()));

            return allMethods.GroupBy(method => new Signature(method))
                             .Select(SignatureWithTheMostDerivedDeclaringType);
        }

        private static MethodInfo SignatureWithTheMostDerivedDeclaringType(IGrouping<Signature, MethodInfo> group)
        {
            return group.Aggregate(
                (a, b) => a.DeclaringType.IsAssignableFrom(b.DeclaringType) ? b : a);
        }

        private sealed class Signature
        {
            private readonly MethodInfo method;

            public Signature(MethodInfo method)
            {
                this.method = method;
            }

            public override bool Equals(object obj)
            {
                var that = obj as Signature;

                if (that == null)
                    return false;

                //different names, therefore different signatures.
                if (this.method.Name != that.method.Name)
                    return false;

                var thisParams = this.method.GetParameters();
                var thatParams = that.method.GetParameters();

                //different number of parameters, therefore different signatures
                if (thisParams.Length != thatParams.Length)
                    return false;

                //different paramaters, therefore different signatures
                for (int i = 0; i < thisParams.Length; i++)
                    if (!AreParamsEqual(thisParams[i], thatParams[i]))
                        return false;

                return true;
            }

            /// <summary>
            /// Two parameters are equal if they have the same type and
            /// they're either both "out" parameters or "non-out" parameters.
            /// </summary>
            private bool AreParamsEqual(ParameterInfo x, ParameterInfo y)
            {
                return x.ParameterType == y.ParameterType &&
                       x.IsOut == y.IsOut;
            }

            public override int GetHashCode()
            {
                int hash = 37;
                hash = hash*23 + method.Name.GetHashCode();

                foreach (var p in method.GetParameters())
                {
                    hash = hash*23 + p.ParameterType.GetHashCode();
                    hash = hash*23 + p.IsOut.GetHashCode();
                }
                return hash;
            }
        }
    }
}
