using System.Collections.Generic;
using System.Linq;
using Integration;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[DotNetVerbosityMapping]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;
    [BuildTrigger] readonly BuildTrigger Trigger;

    [PackageExecutable("NUnit.Runners", "nunit-console.exe")]
    readonly Tool NUnit2Runner;

    [EnvironmentVariable("NUGET_API_KEY")] readonly string NuGetApiKey;
    readonly string NuGetSource = "https://api.nuget.org/v3/index.json";

    [EnvironmentVariable("MYGET_API_KEY")] readonly string MyGetApiKey;
    readonly string MyGetSource = "https://api.nuget.org/v3/index.json";

    [Partition(2)] readonly Partition TestPartition;

    IEnumerable<Project> TestProjects
        => TestPartition.GetCurrent(Solution.GetProjects("*Test"));

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory / "testresults";
    AbsolutePath CoverageDirectory => ArtifactsDirectory / "coverage";
    AbsolutePath ReportsDirectory => ArtifactsDirectory / "reports";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory
                .GlobDirectories("**/bin", "**/obj")
                .ForEach(DeleteDirectory);

            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .SetProcessArgumentConfigurator(a => a
                    .Add("/p:CheckEolTargetFramework=false")));
        });

    Target Verify => _ => _
        .DependsOn(Restore)
        .Before(Compile)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration("Verify")
                .SetNoRestore(FinishedTargets.Contains(Restore))
                .SetProcessArgumentConfigurator(a => a
                    .Add("/p:CheckEolTargetFramework=false")));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetNoRestore(FinishedTargets.Contains(Restore))
                .SetProcessArgumentConfigurator(a => a
                    .Add("/p:CheckEolTargetFramework=false")));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Produces(TestResultsDirectory / "*.zip")
        .Produces(CoverageDirectory / "*.zip")
        .Partition(() => TestPartition)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .SetNoBuild(FinishedTargets.Contains(Compile))
                .ResetVerbosity()
                .SetResultsDirectory(TestResultsDirectory)
                .SetProcessArgumentConfigurator(a => a
                    .Add("/p:CheckEolTargetFramework=false")
                    .Add("-- RunConfiguration.DisableAppDomain=true")
                    .Add("-- RunConfiguration.NoAutoReporters=true"))
                .When(InvokedTargets.Contains(Cover), _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                    .SetExcludeByFile("**/TestTypeFoundation/**")
                    .When(IsServerBuild, _ => _.EnableUseSourceLink()))
                .CombineWith(TestProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLogger("trx")
                    .When(InvokedTargets.Contains(Cover), _ => _
                        .SetCoverletOutput(CoverageDirectory / $"{v.Name}.xml"))));

            CompressZip(TestResultsDirectory, TestResultsDirectory / "TestResults.zip");
            CompressZip(CoverageDirectory, CoverageDirectory / "CoverageResults.zip");

            AppVeyor?.PushArtifact(TestResultsDirectory / "TestResults.zip");
            AppVeyor?.PushArtifact(CoverageDirectory / "CoverageResults.zip");
        });

    Target Cover => _ => _
        .DependsOn(Test)
        .Consumes(Test)
        .Before(Pack)
        .Produces(ReportsDirectory / "*.zip")
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetFramework("netcoreapp2.1")
                .SetReports(CoverageDirectory / "*.xml")
                .SetTargetDirectory(ReportsDirectory)
                .SetReportTypes("lcov", ReportTypes.HtmlInline));

            CompressZip(ReportsDirectory, ReportsDirectory / "CoverageReport.zip");
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Consumes(Compile)
        .After(Test)
        .Produces(PackagesDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackagesDirectory)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .EnableIncludeSymbols()
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetProcessArgumentConfigurator(a => a
                    .Add("/p:CheckEolTargetFramework=false")));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => IsServerBuild)
        .OnlyWhenDynamic(() => AppVeyor != null && Trigger == BuildTrigger.SemVerTag)
        .Consumes(Pack)
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .EnableSkipDuplicate()
                .When(
                    GitRepository.IsOnMasterBranch(),
                    v => v
                        .SetApiKey(NuGetApiKey)
                        .SetSource(NuGetSource))
                .When(
                    !GitRepository.IsOnMasterBranch(),
                    v => v
                        .SetApiKey(MyGetApiKey)
                        .SetApiKey(MyGetSource)));
        });
}