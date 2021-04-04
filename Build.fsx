#r "paket:
nuget Fake.BuildServer.AppVeyor
nuget Fake.Core.Environment
nuget Fake.Core.Target
nuget Fake.Core.Xml
nuget Fake.DotNet
nuget Fake.DotNet.Cli
nuget Fake.DotNet.MsBuild
nuget Fake.DotNet.NuGet
nuget Fake.DotNet.Testing.NUnit
nuget Fake.IO.FileSystem
nuget Fake.IO.Zip
nuget Fake.Tools.Git
nuget Fake.Testing.Common
"

#load ".fake/build.fsx/intellisense.fsx"

open System
open System.Text.RegularExpressions

open Fake.BuildServer;
open Fake.Core;
open Fake.Core.String.Operators
open Fake.Core.TargetOperators
open Fake.DotNet;
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.Tools;

let buildDir = Environment.environVarOrDefault "BUILD_DIR" "build" |> Path.getFullName
let buildToolsDir = buildDir </> "tools"
let testResultsFolder = buildDir </> "TestResults" |> Path.getFullName
let nuGetOutputFolder = buildDir </> "NuGetPackages"
let nuGetPackages = !! (nuGetOutputFolder </> "*.nupkg")
let sourcesDirPath = "src"
let solutionPath = sourcesDirPath </> "All.sln" |> Path.getFullName
let buildConfiguration = DotNet.BuildConfiguration.fromEnvironVarOrDefault "BUILD_CONFIGURATION" DotNet.BuildConfiguration.Release
let buildVerbosity = match Environment.environVarOrDefault "BUILD_VERBOSITY" ""  |> String.toLower with
                     | "quiet" | "q"         -> Quiet
                     | "minimal" | "m"       -> Minimal
                     | "normal" | "n"        -> Normal
                     | "detailed" | "d"      -> Detailed
                     | "diagnostic" | "diag" -> Diagnostic
                     | _ -> Minimal


type BuildVersionCalculationSource = { Major: int; Minor: int; Revision: int; PreSuffix: string; 
                                       CommitsNum: int; Sha: string; BuildNumber: int }
let getVersionSourceFromGit buildNumber =
    // The --fist-parent flag is required to correctly work for vNext branch.
    // Example of output for a release tag: v3.50.2-288-g64fd5c5b, for a prerelease tag: v3.50.2-alpha1-288-g64fd5c5b.
    let desc = Git.CommandHelper.runSimpleGitCommand "" "describe --tags --long --abbrev=40 --first-parent --match=v*"

    // Previously repository contained a few broken tags like "v.3.21.1". They were removed, but could still exist
    // in forks. We handle them as well to not fail on such repositories.
    let result = Regex.Match(desc,
                             @"^v(\.)?(?<maj>\d+)\.(?<min>\d+)\.(?<rev>\d+)(?<pre>-\w+\d*)?-(?<num>\d+)-g(?<sha>[a-z0-9]+)$",
                             RegexOptions.IgnoreCase)
                      .Groups

    let getMatch (name:string) = result.[name].Value

    { Major = getMatch "maj" |> int
      Minor = getMatch "min" |> int
      Revision = getMatch "rev" |> int
      PreSuffix = getMatch "pre"
      CommitsNum = getMatch "num" |> int
      Sha = getMatch "sha"
      BuildNumber = buildNumber
    }

type BuildVersionInfo = { AssemblyVersion:string; FileVersion:string; InfoVersion:string; NugetVersion:string; CommitHash: string;
                          Source: Option<BuildVersionCalculationSource> }
let calculateVersion source =
    let s = source
    let (major, minor, revision, preReleaseSuffix, commitsNum, sha, buildNumber) =
        (s.Major, s.Minor, s.Revision, s.PreSuffix, s.CommitsNum, s.Sha, s.BuildNumber)

    let assemblyVersion = sprintf "%d.%d.0.0" major minor
    let fileVersion = sprintf "%d.%d.%d.%d" major minor revision buildNumber
    
    // If number of commits since last tag is greater than zero, we append another identifier with number of commits.
    // The produced version is larger than the last tag version.
    // If we are on a tag, we use version without modification.
    // Examples of output: 3.50.2.1, 3.50.2.215, 3.50.1-rc1.3, 3.50.1-rc3.35
    let nugetVersion = match commitsNum with
                       | 0 -> sprintf "%d.%d.%d%s" major minor revision preReleaseSuffix
                       | _ -> sprintf "%d.%d.%d%s.%d" major minor revision preReleaseSuffix commitsNum

    let infoVersion = match commitsNum with
                      | 0 -> nugetVersion
                      | _ -> sprintf "%s-%s" nugetVersion sha

    { AssemblyVersion=assemblyVersion; FileVersion=fileVersion; InfoVersion=infoVersion; NugetVersion=nugetVersion; CommitHash=s.Sha;
      Source = Some source }

// Calculate version that should be used for the build. Define globally as data might be required by multiple targets.
// Please never name the build parameter with version as "Version" - it might be consumed by the MSBuild, override 
// the defined properties and break some tasks (e.g. NuGet restore).
let mutable buildVersion = match Environment.environVarOrDefault "BUILD_VERSION" "git" with
                           | "git"       -> Environment.environVarOrDefault "BUILD_NUMBER" "0"
                                            |> int
                                            |> getVersionSourceFromGit
                                            |> calculateVersion

                           | assemblyVer -> { AssemblyVersion = assemblyVer
                                              FileVersion = Environment.environVarOrDefault "BUILD_FILE_VERSION" assemblyVer
                                              InfoVersion = Environment.environVarOrDefault "BUILD_INFO_VERSION" assemblyVer
                                              NugetVersion = Environment.environVarOrDefault "BUILD_NUGET_VERSION" assemblyVer
                                              CommitHash = Environment.environVarOrDefault "BUILD_COMMIT_HASH" ""
                                              Source = None }

let setVNextBranchVersion vNextVersion =
    buildVersion <-
        match buildVersion.Source with
        // Don't update version if it was explicitly specified.
        | None                                -> buildVersion
        // Don't update version if tag with current major version is already present (e.g. rc is released).
        | Some s when s.Major >= vNextVersion -> buildVersion
        | Some source                         -> 
            // The trick is the "git describe" command contains the --first-parent flag.
            // Because of that git matched the last release tag before the fork was created and calculated number
            // of commits since that release. We are perfectly fine, as this number will constantly increase only.
            // Set version to X.0.0-alpha.NUM, where X - major version, NUM - commits since last release before fork.
            { source with Major = vNextVersion
                          Minor = 0
                          Revision = 0
                          PreSuffix = "-alpha" }
            |> calculateVersion

let configureMsBuildParams (parameters: MSBuild.CliArguments) = 
    let isCiBuild = BuildServer.buildServer <> LocalBuild

    let properties = [ "AssemblyVersion", buildVersion.AssemblyVersion
                       "FileVersion", buildVersion.FileVersion
                       "InformationalVersion", buildVersion.InfoVersion
                       "PackageVersion", buildVersion.NugetVersion
                       "CommitHash", buildVersion.CommitHash
                       "EnableSourceLink", isCiBuild.ToString()
                       "ContinuousIntegrationBuild", isCiBuild.ToString() ]

    { parameters with Verbosity = Some buildVerbosity
                      Properties = properties }

Target.create "Verify" (fun _ ->
    try
        DotNet.build (fun p -> { p with Configuration = DotNet.BuildConfiguration.Custom "Verify"
                                        MSBuildParams = configureMsBuildParams p.MSBuildParams })
                     solutionPath
    with
    | MSBuildException (msg, errors) -> 
        let msg = sprintf
                    "%s\r\nHINT: To simplify the fix process it's recommended to switch to the 'Verify' configuration \
                    in the IDE. This way you might get Roslyn quick fixes for the violated rules."
                    msg
        raise (MSBuildException(msg, errors))
)

Target.create "Build" (fun _ ->
    DotNet.build (fun p -> { p with Configuration = buildConfiguration
                                    MSBuildParams = configureMsBuildParams p.MSBuildParams })
                 solutionPath
)

Target.create "CleanTestResultsFolder" (fun _ -> Shell.cleanDir testResultsFolder)

Target.create "Test" (fun _ ->
    let findProjects pattern = System.IO.Directory.GetDirectories("Src", pattern)
    let getTestAssemblies framework projDirs =
        projDirs
        |> Seq.map (fun proj -> !! (sprintf "bin/%s/%s/*Test.dll" (buildConfiguration.ToString()) framework)
                                |> GlobbingPattern.setBaseDir proj)
        |> Seq.collect id


    // DotNet.test does not support -- parameters for now.
    let result = DotNet.exec id
                    "test"
                    (sprintf 
                        "%s --no-build --configuration %s --logger:trx --results-directory \"%s\" -- RunConfiguration.NoAutoReporters=true"
                        solutionPath
                        (buildConfiguration.ToString())
                        testResultsFolder)

    if not result.OK then failwith "test failed"

    findProjects "AutoFixture.NUnit2.*Test"
    |> getTestAssemblies "*"
    |> Testing.NUnit.Sequential.run (fun p -> { p with StopOnError = false
                                                       ShowLabels = false
                                                       OutputFile  = testResultsFolder </> "NUnit2TestResult.xml"
                                                       ToolPath = buildToolsDir </> "NUnit.Runners.2.6.2" </> "tools" })
)

Target.create "CleanNuGetPackages" (fun _ ->
    Shell.cleanDir nuGetOutputFolder
)

Target.create "NuGetPack" (fun _ ->
    DotNet.pack (fun p -> { p with Configuration = buildConfiguration
                                   OutputPath = Some (Path.getFullName nuGetOutputFolder)
                                   MSBuildParams = configureMsBuildParams p.MSBuildParams })
                solutionPath

    let findDependencyNode name (doc:Xml.XmlDocument) =
            doc.SelectSingleNode(sprintf "//*[local-name()='dependency' and @id='%s']" name)

    // Verify that AutoFixture reference is valid.
    let dependencyVersion = !! "AutoFixture.AutoNSubstitute*"
                            |> GlobbingPattern.setBaseDir nuGetOutputFolder
                            |> Seq.head
                            |> Zip.unzipFirstMatchingFileInMemory (fun ze -> ze.Name.EndsWith ".nuspec")
                            |> Xml.createDoc
                            |> findDependencyNode "AutoFixture"
                            |> Xml.getAttribute "version"

    if(buildVersion.NugetVersion <> dependencyVersion) 
        then failwithf "Invalid dependency version in the produced package. Actual: '%s' Expected: '%s'"
                       dependencyVersion
                       buildVersion.NugetVersion 
        else Trace.logfn "Verified the dependency version. Actual: '%s' Expected: '%s'"
                   dependencyVersion
                   buildVersion.NugetVersion
)

let publishPackages packageFeed accessKey =
    // Protect the secret explicitly, even though FAKE should do it.
    // See the discussion: https://github.com/fsharp/FAKE/issues/2526#issuecomment-650567241
    TraceSecrets.register "<api key>" accessKey 

    nuGetPackages
    |> Seq.iter (fun pkg -> DotNet.nugetPush (fun p -> { p with PushParams = { p.PushParams with ApiKey = Some accessKey
                                                                                                 Source = Some packageFeed }})
                                             pkg
    )

Target.create "PublishNuGet" (fun _ ->
    let feed = "https://www.nuget.org/api/v2/package"
    let key = Environment.environVarOrFail "NUGET_API_KEY"

    publishPackages feed key
)

Target.create "PublishMyGet" (fun _ ->
    let packageFeed = "https://www.myget.org/F/autofixture/api/v2/package"
    let key = Environment.environVarOrFail "MYGET_API_KEY"

    publishPackages packageFeed key
)

Target.create "CompleteBuild"   ignore

"Verify" ==> "Build"


"CleanTestResultsFolder" ==> "Test"
"Build"                  ==> "Test"

"CleanNuGetPackages" ==> "NuGetPack"
"Test"               ==> "NuGetPack"

"NuGetPack" ==> "CompleteBuild"

"NuGetPack" ==> "PublishNuGet"

"NuGetPack" ==> "PublishMyGet"

// ==============================================
// ================== AppVeyor ==================
// ==============================================

// Add a helper to identify whether current trigger is PR.
type AppVeyor.Environment with
    static member IsPullRequest = String.isNotNullOrEmpty AppVeyor.Environment.PullRequestNumber

type AppVeyorTrigger = SemVerTag | CustomTag | PR | VNextBranch | Unknown
let anAppVeyorTrigger =
    let tag = if AppVeyor.Environment.RepoTag then Some AppVeyor.Environment.RepoTagName else None
    let isPR = AppVeyor.Environment.IsPullRequest
    let branch = if String.isNotNullOrEmpty AppVeyor.Environment.RepoBranch then Some AppVeyor.Environment.RepoBranch else None

    match tag, isPR, branch with
    | Some t, _, _ when "v\d+.*" >** t -> SemVerTag
    | Some _, _, _                     -> CustomTag
    | _, true, _                       -> PR
    // Branch name should be checked after the PR flag, because for PR it's set to the upstream branch name.
    | _, _, Some br when "v\d+" >** br -> VNextBranch
    | _                                -> Unknown

Target.create "AppVeyor_SetVNextVersion" (fun _ ->
    // vNext branch has the following name: "vX", where X is the next version.
    AppVeyor.Environment.RepoBranch.Substring(1) 
    |> int
    |> setVNextBranchVersion
)

Target.create "AppVeyor_UploadTestReports" (fun _ ->
    let shuffle xs = xs |> Seq.sortBy (fun _ -> Guid())

    !!"*.trx"
    |> GlobbingPattern.setBaseDir testResultsFolder
    |> Seq.map (fun file -> (file, ImportData.Mstest))
    |> Seq.append [(testResultsFolder </> "NUnit2TestResult.xml", ImportData.Nunit NunitDataVersion.Nunit)]
    |> shuffle
    |> Seq.map (fun (file, format) -> async { Trace.publish format file })
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

Target.createFinal "AppVeyor_UpdateVersion" (fun _ ->
    // Artifacts might be deployable, so we update build version to find them later by file version.
    let versionSuffix = if AppVeyor.Environment.IsPullRequest then
                            let appVeyorVersion = AppVeyor.Environment.BuildVersion;
                            appVeyorVersion.Substring(appVeyorVersion.IndexOf('-'))
                        else
                            ""
    Trace.setBuildNumber (buildVersion.FileVersion + versionSuffix)
)

Target.create "AppVeyor" ignore

"AppVeyor_SetVNextVersion" =?> ("PatchAssemblyVersions", anAppVeyorTrigger = VNextBranch)

"Test" ==> "AppVeyor_UploadTestReports"

// Add logic to resolve action based on current trigger info.
(==>) <| match anAppVeyorTrigger with
         | SemVerTag                -> "PublishNuGet"
         | VNextBranch              -> "PublishMyGet"
         | PR | CustomTag | Unknown -> "CompleteBuild"
      <| "AppVeyor"

"AppVeyor_UploadTestReports" ==> "AppVeyor"

// Print state info at the very beginning.
if BuildServer.buildServer = AppVeyor 
   then Trace.logfn "[AppVeyor state] Is tag: %b, tag name: '%s', is PR: %b, branch name: '%s', trigger: %A, build version: '%s'"
              AppVeyor.Environment.RepoTag 
              AppVeyor.Environment.RepoTagName 
              AppVeyor.Environment.IsPullRequest
              AppVeyor.Environment.RepoBranch
              anAppVeyorTrigger
              AppVeyor.Environment.BuildVersion
        Target.activateFinal "AppVeyor_UpdateVersion"

BuildServer.install [ AppVeyor.Installer ]

// ========= ENTRY POINT =========
Target.runOrDefault "CompleteBuild"
