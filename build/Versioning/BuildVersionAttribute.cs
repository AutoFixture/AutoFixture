using System.Reflection;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.ValueInjection;

public class BuildVersionAttribute : ValueInjectionAttributeBase
{
    public override object GetValue(MemberInfo member, object instance)
    {
        var version = GitBasedVersion.Calculate(AppVeyor.Instance?.BuildNumber ?? 0);

        AppVeyor.Instance?.UpdateBuildVersion(version.FileVersion);

        return version;
    }
}