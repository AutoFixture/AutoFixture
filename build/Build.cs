using System.Collections.Generic;
using Integration;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using Versioning;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[DotNetVerbosityMapping]
[AppVeyor(
    AppVeyorImage.VisualStudioLatest,
    AutoGenerate = false,
    InvokedTargets = new [] {nameof(Cover), nameof(Pack)})]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [CI] readonly AppVeyor AppVeyor;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [BuildVersion] readonly BuildVersionInfo BuildVersion;
    [BuildTrigger] readonly BuildTrigger Trigger;

    [EnvironmentVariable("NUGET_API_KEY")]
    readonly string NuGetApiKey;
    readonly string NuGetSource = "https://api.nuget.org/v3/index.json";

    [EnvironmentVariable("MYGET_API_KEY")]
    readonly string MyGetApiKey;
    readonly string MyGetSource = "https://api.nuget.org/v3/index.json";

    [Partition(2)]
    readonly Partition TestPartition;

    IEnumerable<Project> TestProjects
        => TestPartition.GetCurrent(Solution.GetProjects("*Test"));

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory / "testresults";
    AbsolutePath CoverageDirectory => ArtifactsDirectory / "coverage";
    AbsolutePath ReportsDirectory => ArtifactsDirectory / "reports";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";

    Target Setup => _ => _
        .Executes(() =>
        {
            if (Trigger != BuildTrigger.PullRequest)
            {
                AppVeyor?.UpdateBuildVersion(BuildVersion.FileVersion);
            }
        });
    
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
        .DependsOn(Setup)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVersion(BuildVersion.NuGetVersion)
                .SetAssemblyVersion(BuildVersion.AssemblyVersion)
                .SetFileVersion(BuildVersion.FileVersion)
                .SetInformationalVersion(BuildVersion.InfoVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Produces(TestResultsDirectory / "*.trx")
        .Produces(CoverageDirectory / "*.xml")
        .Partition(() => TestPartition)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .ResetVerbosity()
                .SetResultsDirectory(TestResultsDirectory)
                .SetProcessArgumentConfigurator(a => a
                    .Add("-- RunConfiguration.DisableAppDomain=true")
                    .Add("-- RunConfiguration.NoAutoReporters=true"))
                .When(InvokedTargets.Contains(Cover), _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                    .SetExcludeByFile("**/TestTypeFoundation/**")
                    .When(IsServerBuild, _ => _.EnableUseSourceLink()))
                .CombineWith(TestProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLogger($"trx;LogFileName={v.Name}.trx")
                    .When(InvokedTargets.Contains(Cover), _ => _
                        .SetCoverletOutput(CoverageDirectory / $"{v.Name}.xml"))));

            AppVeyor?.UploadTestResults(TestType.MsTest, TestResultsDirectory, "*.trx");
        });

    Target Cover => _ => _
        .DependsOn(Test)
        .Consumes(Test)
        .Produces(ReportsDirectory / "lcov.info")
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetFramework("netcoreapp2.1")
                .SetReports(CoverageDirectory / "*.xml")
                .SetTargetDirectory(ReportsDirectory)
                .SetReportTypes((ReportTypes)"lcov")
                .When(IsLocalBuild, _ => _
                    .AddReportTypes(ReportTypes.HtmlInline)));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Consumes(Compile)
        .Produces(PackagesDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackagesDirectory)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .EnableIncludeSymbols()
                .SetVersion(BuildVersion.NuGetVersion)
                .SetAssemblyVersion(BuildVersion.AssemblyVersion)
                .SetFileVersion(BuildVersion.FileVersion)
                .SetInformationalVersion(BuildVersion.InfoVersion));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => IsServerBuild)
        .OnlyWhenDynamic(() => Trigger == BuildTrigger.SemVerTag)
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
