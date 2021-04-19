using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    internal class ReflectedSource : ITestCaseSource
    {
        private readonly Lazy<ITestCaseSource> lazySource;

        public ReflectedSource(Type type, string memberName, IEnumerable<object> parameters)
        {
            this.Type = type;
            this.MemberName = memberName;
            this.Parameters = parameters;
            this.lazySource = new Lazy<ITestCaseSource>(() => GetSource(this.Type, this.MemberName, this.Parameters));
        }

        public Type Type { get; }

        public string MemberName { get; }

        public IEnumerable<object> Parameters { get; }

        public IEnumerable<IReadOnlyList<object>> GetTestCases(IMethodInfo method)
        {
            return this.lazySource.Value.GetTestCases(method);
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningBracesMustBeSpacedCorrectly",
            Justification = "This section makes of use of the new pattern matching syntax.")]
        private static ITestCaseSource GetSource(Type type, string name, IEnumerable<object> parameters)
        {
            return (type, name) switch
            {
                ({ } t, { } n) => new StaticMemberSource(t, n, parameters),
                ({ } t, _) => new ClassTestCaseSource(t),
                (_, _) => new NullSource()
            };
        }
    }
}
