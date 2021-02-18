using System.Reflection;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.ValueInjection;

namespace Versioning
{
    public sealed class BuildVersionAttribute : ValueInjectionAttributeBase
    {
        public override object GetValue(MemberInfo member, object instance)
            => GitBasedVersion.Calculate(AppVeyor.Instance?.BuildNumber ?? 0);
    }
}