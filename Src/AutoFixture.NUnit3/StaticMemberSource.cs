using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    internal class StaticMemberSource : ITestCaseSource
    {
        private readonly Lazy<ITestCaseSource> lazySource;
        private const BindingFlags Binding = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public StaticMemberSource(Type type, string name, IEnumerable<object> parameters)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Parameters = parameters;
            this.lazySource = new Lazy<ITestCaseSource>(() => GetSource(this.Type, this.Name, this.Parameters));
        }

        public Type Type { get; }

        public string Name { get; }

        public IEnumerable<object> Parameters { get; }

        public IEnumerable<IReadOnlyList<object>> GetTestCases(IMethodInfo method)
        {
            return this.lazySource.Value.GetTestCases(method);
        }

        private static ITestCaseSource GetSource(Type type, string name, IEnumerable<object> parameters)
        {
            return type.GetTypeInfo().GetMember(name, Binding).Single() switch
            {
                MethodInfo m => new StaticMethodSource(m, parameters),
                PropertyInfo p => new StaticPropertySource(p),
                FieldInfo f => new StaticFieldSource(f),
                _ => new NullSource()
            };
        }
    }
}