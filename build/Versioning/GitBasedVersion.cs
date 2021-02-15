using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common.Tools.Git;

public static class GitBasedVersion
{
    public static BuildVersionInfo Calculate(int buildNumber)
    {
        var desc = GitTasks.Git("describe --tags --long --abbrev=40 --match=v*", logInvocation: false, logOutput: false)
            .Single().Text;
        var result = Regex.Match(desc,
                @"^v(?<maj>\d+)\.(?<min>\d+)(\.(?<rev>\d+))?(?<pre>-\w+\d*)?-(?<num>\d+)-g(?<sha>[a-z0-9]+)$",
                RegexOptions.IgnoreCase)
            .Groups;

        string GetMatch(string name) => result[name].Value;

        var major = int.Parse(GetMatch("maj"));
        var minor = int.Parse(GetMatch("min"));
        var optRevision = GetMatch("rev");
        var optPreReleaseSuffix = GetMatch("pre");
        var commitsNum = int.Parse(GetMatch("num"));
        var sha = GetMatch("sha");

        var revision = optRevision == "" ? 0 : int.Parse(optRevision);
        var assemblyVersion = $"{major}.{minor}.{revision}.0";
        var fileVersion = $"{major}.{minor}.{revision}.{buildNumber}";

        // If number of commits since last tag is greater than zero, we append another identifier with number of commits.
        // The produced version is larger than the last tag version.
        // If we are on a tag, we use version specified modification.
        var nugetVersion = commitsNum switch
        {
            0 => $"{major}.{minor}.{revision}{optPreReleaseSuffix}",
            var num => $"{major}.{minor}.{revision}{optPreReleaseSuffix}.{num}",
        };

        var infoVersion = commitsNum switch
        {
            0 => nugetVersion,
            _ => $"{nugetVersion}-{sha}"
        };

        return new BuildVersionInfo
        {
            AssemblyVersion = assemblyVersion,
            FileVersion = fileVersion,
            InfoVersion = infoVersion,
            NuGetVersion = nugetVersion
        };
    }
}