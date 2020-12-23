using System;
using System.Collections.Generic;
using System.Linq;
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
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;
    
    [Parameter("NuGet API Key")]
    readonly string NuGetApiKey;
    [Parameter("NuGet Source for Packages")]
    readonly string NuGetSource = "https://api.nuget.org/v3/index.json";
    
    [Parameter("MyGet API Key")]
    readonly string MyGetApiKey;
    [Parameter("MyGet Source for Packages")]
    readonly string MyGetSource = "https://api.nuget.org/v3/index.json";

    [Partition(2)]
    readonly Partition TestPartition;

    IEnumerable<Project> TestProjects => TestPartition.GetCurrent(Solution.GetProjects("*Test"));

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
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
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
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackagesDirectory)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .EnableIncludeSymbols()
                .SetProject(Solution));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Consumes(Pack)
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .EnableSkipDuplicate()
                .When(
                    IsServerBuild && GitRepository.IsOnMasterBranch(),
                    v => v
                        .SetApiKey(NuGetApiKey)
                        .SetSource(NuGetSource))
                .When(
                    IsServerBuild && !GitRepository.IsOnMasterBranch(),
                    v => v
                        .SetApiKey(MyGetApiKey)
                        .SetApiKey(MyGetSource)));
        });
}
