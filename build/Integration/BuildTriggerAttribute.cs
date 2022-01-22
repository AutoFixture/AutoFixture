using System;
using System.Reflection;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.ValueInjection;

namespace Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BuildTriggerAttribute : ValueInjectionAttributeBase
    {
        public override object GetValue(MemberInfo member, object instance)
            => AppVeyor.Instance.GetTrigger();
    }
}