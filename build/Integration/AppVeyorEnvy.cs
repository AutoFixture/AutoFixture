using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.RegularExpressions;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace Integration
{
    public static class AppVeyorEnvy
    {
        public static BuildTrigger GetTrigger(this AppVeyor environment)
        {
            if (environment == null) return BuildTrigger.Invalid;

            var tag = environment.RepositoryTag ? environment.RepositoryTagName : null;
            var isPr = environment.PullRequestNumber.HasValue;
            var branchName = environment.RepositoryBranch;

            return (tag, isPr, branchName) switch
            {
                ({ } t, _, _) when Regex.IsMatch(t, "^v\\d.*") => BuildTrigger.SemVerTag,
                (_, true, _)                                   => BuildTrigger.PullRequest,
                (_, _, "master")                               => BuildTrigger.MasterBranch,
                _                                              => BuildTrigger.UnknownBranchOrTag
            };
        }

        public static void UploadTestResults([NotNull] this AppVeyor environment,
            TestType type, AbsolutePath directory, params string[] patterns)
        {
            var testType = GetType(type);
            var address = $"https://ci.appveyor.com/api/testresults/{testType}/{environment.JobId}";
            var wc = new WebClient();

            directory
                .GlobFiles(patterns)
                .ForEach(x => wc.UploadFile(address, x));
        }

        static string GetType(TestType type)
            => type switch
            {
                TestType.MsTest => "mstest",
                TestType.XUnit => "xunit",
                TestType.NUnit => "nunit",
                TestType.NUnit3 => "nunit3",
                _ => "mstest"
            };
    }
}