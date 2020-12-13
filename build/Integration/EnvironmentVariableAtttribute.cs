using System;
using System.Reflection;
using JetBrains.Annotations;
using Nuke.Common.ValueInjection;

namespace Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EnvironmentVariableAttribute : ValueInjectionAttributeBase
    {
        readonly string Name;

        public EnvironmentVariableAttribute([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override object GetValue(MemberInfo member, object instance)
            => Environment.GetEnvironmentVariable(Name);
    }
}