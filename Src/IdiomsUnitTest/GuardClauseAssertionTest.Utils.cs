using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public partial class GuardClauseAssertionTest
    {
        private static IEnumerable<object> GetParameters(IGuardClauseCommand commmand)
        {
            var methodInvokeCommand = (MethodInvokeCommand)((ReflectionExceptionUnwrappingCommand)commmand).Command;
            var indexedReplacement = (IndexedReplacement<object>)methodInvokeCommand.Expansion;
            return indexedReplacement.Source;
        }

        /// <summary>
        /// Wrapper around member to produce nice theory name.
        /// </summary>
        public class MemberRef<T>
            where T : MemberInfo
        {
            public MemberRef(T member)
            {
                this.Member = member;
            }

            public T Member { get; }

            public override string ToString()
            {
                var str = new StringBuilder();
                str.Append(GetNonMangledTypeName(this.Member.DeclaringType));
                str.Append('.');

                str.Append(this.Member.Name);

                var methodBase = this.Member as MethodBase;
                if (methodBase != null)
                {
                    str.Append('(');
                    str.Append(string.Join(", ",
                        methodBase.GetParameters().Select(p => GetNonMangledTypeName(p.ParameterType))));
                    str.Append(')');
                }

                return str.ToString();
            }

            private static string GetNonMangledTypeName(Type type)
            {
                var typeName = type.Name;
                if (!type.GetTypeInfo().IsGenericType)
                    return typeName;

                typeName = typeName.Substring(0, typeName.IndexOf('`'));
                var genericArgTypes = type.GetGenericArguments().Select(GetNonMangledTypeName);
                return string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", typeName, string.Join(", ", genericArgTypes));
            }
        }

        public static class MemberRef
        {
            public static MemberRef<MethodInfo> MethodByName(Type type, string methodName)
            {
                return new MemberRef<MethodInfo>(type.GetMethod(methodName));
            }

            public static MemberRef<MethodInfo> MethodByIndex(Type type, int index)
            {
                return new MemberRef<MethodInfo>(type.GetMethods().Where(IsNotEqualsMethod).ElementAt(index));
            }

            public static MemberRef<ConstructorInfo> CtorByArgs(Type type, Type[] ctorParams)
            {
                return new MemberRef<ConstructorInfo>(type.GetConstructor(ctorParams));
            }

            private static bool IsNotEqualsMethod(MethodInfo method)
            {
                return method.Name != "Equals";
            }
        }

        private static TheoryData<T> MakeTheoryData<T>(IEnumerable<T> entries)
        {
            var result = new TheoryData<T>();

            foreach (var entry in entries)
            {
                result.Add(entry);
            }

            return result;
        }
    }
}