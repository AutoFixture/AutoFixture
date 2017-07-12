#r @"tools/FAKE.Core/tools/FakeLib.dll"

open Fake
open Fake.AppVeyor
open Fake.DotNetCli
open Fake.Testing
open System
open System.Diagnostics;
open System.Text.RegularExpressions

let releaseFolder = "Release"
let nuGetRestoreFolder = "Packages"
let nuGetOutputFolder = "NuGetPackages"
let nuGetPackages = !! (nuGetOutputFolder @@ "*.nupkg" )
                    // Skip symbol packages because NuGet publish symbols automatically when package is published
                    -- (nuGetOutputFolder @@ "*.symbols.nupkg")
                    // Currently AutoFakeItEasy2 has been deprecated and is not being published to the feeds.
                    -- (nuGetOutputFolder @@ "AutoFixture.AutoFakeItEasy2.*" )
let signKeyPath = FullName "Src/AutoFixture.snk"
let solutionsToBuild = !! "Src/All.sln"
let bakFileExt = ".orig"

type BuildVersionCalculationSource = { major: int; minor: int; revision: int; preSuffix: string; 
                                       commitsNum: int; sha: string; buildNumber: int }
let getVersionSourceFromGit buildNumber =
    // The --fist-parent flag is required to correctly work for vNext branch.
    // Example of output for a release tag: v3.50.2-288-g64fd5c5b, for a prerelease tag: v3.50.2-alpha1-288-g64fd5c5b
    let desc = Git.CommandHelper.runSimpleGitCommand "" "describe --tags --long --first-parent --match=v*"

    // Previously repository contained a few broken tags like "v.3.21.1". They were removed, but could still exist
    // in forks. We handle them as well to not fail on such repositories.
    let result = Regex.Match(desc,
                             @"^v(\.)?(?<maj>\d+)\.(?<min>\d+)\.(?<rev>\d+)(?<pre>-\w+\d*)?-(?<num>\d+)-g(?<sha>[a-z0-9]+)$",
                             RegexOptions.IgnoreCase)
                      .Groups

    let getMatch (name:string) = result.[name].Value

    { major = getMatch "maj" |> int
      minor = getMatch "min" |> int
      revision = getMatch "rev" |> int
      preSuffix = getMatch "pre"
      commitsNum = getMatch "num" |> int
      sha = getMatch "sha"
      buildNumber = buildNumber
    }

type BuildVersionInfo = { assemblyVersion:string; fileVersion:string; infoVersion:string; nugetVersion:string; 
                          source: Option<BuildVersionCalculationSource> }
let calculateVersion source =
    let s = source
    let (major, minor, revision, preReleaseSuffix, commitsNum, sha, buildNumber) =
        (s.major, s.minor, s.revision, s.preSuffix, s.commitsNum, s.sha, s.buildNumber)

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

    { assemblyVersion=assemblyVersion; fileVersion=fileVersion; infoVersion=infoVersion; nugetVersion=nugetVersion; 
      source = Some source }

// Calculate version that should be used for the build. Define globally as data might be required by multiple targets.
// Please never name the build parameter with version as "Version" - it might be consumed by the MSBuild, override 
// the defined properties and break some tasks (e.g. NuGet restore).
let mutable buildVersion = match getBuildParamOrDefault "BuildVersion" "git" with
                           | "git"       -> getBuildParamOrDefault "BuildNumber" "0"
                                            |> int
                                            |> getVersionSourceFromGit
                                            |> calculateVersion

                           | assemblyVer -> { assemblyVersion = assemblyVer
                                              fileVersion = getBuildParamOrDefault "BuildFileVersion" assemblyVer
                                              infoVersion = getBuildParamOrDefault "BuildInfoVersion" assemblyVer
                                              nugetVersion = getBuildParamOrDefault "BuildNugetVersion" assemblyVer
                                              source = None }

let setVNextBranchVersion vNextVersion =
    buildVersion <-
        match buildVersion.source with
        // Don't update version if it was explicitly specified
        | None                                -> buildVersion
        // Don't update version if tag with current major version is already present (e.g. rc is released)
        | Some s when s.major >= vNextVersion -> buildVersion
        | Some source                         -> 
            // The trick is the "git describe" command contains the --first-parent flag.
            // Because of that git matched the last release tag before the fork was created and calculated number
            // of commits since that release. We are perfectly fine, as this number will constantly increase only.
            // Set version to X.0.0-alpha.NUM, where X - major version, NUM - commits since last release before fork
            { source with major = vNextVersion
                          minor = 0
                          revision = 0
                          preSuffix = "-alpha" }
            |> calculateVersion

let addBakExt path = sprintf "%s%s" path bakFileExt

Target "PatchAssemblyVersions" (fun _ ->
    printfn 
        "Patching assembly versions. Assembly version: %s, File version: %s, Informational version: %s" 
        buildVersion.assemblyVersion
        buildVersion.fileVersion
        buildVersion.infoVersion

    let filesToPatch = !! "Src/*/Properties/AssemblyInfo.*"
                       -- addBakExt "Src/*/Properties/*"
    
    // Backup the original file versions
    filesToPatch
    |> Seq.iter (fun f ->
        let bakFilePath = addBakExt f
        CopyFile bakFilePath f

        printfn "Backed up %s to %s" f bakFilePath
    )

    ReplaceAssemblyInfoVersionsBulk filesToPatch 
                                    (fun f -> { f with AssemblyVersion              = buildVersion.assemblyVersion
                                                       AssemblyFileVersion          = buildVersion.fileVersion
                                                       AssemblyInformationalVersion = buildVersion.infoVersion })
)

Target "RestorePatchedAssemblyVersionFiles" (fun _ ->
    !! (addBakExt "Src/*/Properties/AssemblyInfo.*")
    |> Seq.iter (fun bakFile ->
        let originalPath = bakFile.Substring(0, bakFile.Length - bakFileExt.Length)

        DeleteFile originalPath
        Rename originalPath bakFile

        printfn "Restored %s to %s" bakFile originalPath
    )
)

let build target configuration additionalProperties =
    let properties = [ "Configuration", configuration
                       "AssemblyOriginatorKeyFile", signKeyPath
                       "AssemblyVersion", buildVersion.assemblyVersion
                       "FileVersion", buildVersion.fileVersion
                       "InformationalVersion", buildVersion.infoVersion 
                       "PackageVersion", buildVersion.nugetVersion ]
                     @ additionalProperties

    solutionsToBuild
    |> MSBuild "" target properties
    |> ignore

let clean configuration   = build "Clean" configuration []
let rebuild configuration = build "Rebuild" configuration []

Target "CleanAll"           DoNothing 
Target "CleanVerify"        (fun _ -> clean "Verify")
Target "CleanRelease"       (fun _ -> clean "Release")
Target "CleanReleaseFolder" (fun _ -> CleanDir releaseFolder)

Target "RestoreNuGetPackages" (fun _ ->
    solutionsToBuild
    |> Seq.iter (RestoreMSSolutionPackages (fun p -> { p with OutputPath = nuGetRestoreFolder }))
)

Target "Verify" (fun _ -> rebuild "Verify")

Target "BuildOnly" (fun _ -> rebuild "Release")
Target "TestOnly" (fun _ ->
    let configuration = getBuildParamOrDefault "Configuration" "Release"
    let parallelizeTests = getBuildParamOrDefault "ParallelizeTests" "False" |> Convert.ToBoolean
    let maxParallelThreads = getBuildParamOrDefault "MaxParallelThreads" "0" |> Convert.ToInt32
    let parallelMode = if parallelizeTests then ParallelMode.All else ParallelMode.NoParallelization
    let maxThreads = if maxParallelThreads = 0 then CollectionConcurrencyMode.Default else CollectionConcurrencyMode.MaxThreads(maxParallelThreads)

    let netCoreXunitProjects = [
        "AutoFixtureUnitTest"
        "AutoFixtureDocumentationTest"
    ]

    let testAssemblies = !! (sprintf "Src/*Test/bin/%s/**/*Test.dll" configuration)
                         -- (sprintf "Src/AutoFixture.NUnit*.*Test/bin/%s/**/*Test.dll" configuration)

    let xUnitConsoleAssemblies = 
        netCoreXunitProjects
        |> Seq.fold (fun result proj ->  result -- (sprintf "Src/%s/bin/%s/**/*.dll" proj configuration))
                    testAssemblies

    xUnitConsoleAssemblies
    |> xUnit2 (fun p -> { p with Parallel = parallelMode
                                 MaxThreads = maxThreads })
    netCoreXunitProjects
    |> Seq.map (fun p -> sprintf "Src/%s" p)
    |> Seq.iter (fun projDir -> 
        DotNetCli.RunCommand (fun p -> { p with WorkingDir = projDir })
                             (sprintf "xunit -nobuild -configuration %s" configuration)
    )

    let nunit2TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit2.*Test/bin/%s/**/*Test.dll" configuration)

    nunit2TestAssemblies
    |> NUnitSequential.NUnit (fun p -> { p with StopOnError = false
                                                OutputFile = "NUnit2TestResult.xml" })

    let nunit3TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit3.*Test/bin/%s/**/*Test.dll" configuration)

    nunit3TestAssemblies
    |> Testing.NUnit3.NUnit3 (fun p -> { p with StopOnError = false
                                                ResultSpecs = ["NUnit3TestResult.xml;format=nunit2"] })
)

Target "BuildAndTestOnly" DoNothing
Target "Build" DoNothing
Target "Test"  DoNothing

Target "CopyToReleaseFolder" (fun _ ->
    let buildOutput = [
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.dll";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.pdb";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.XML";
      "Src/AutoFixture.NUnit2/bin/Release/net45/Ploeh.AutoFixture.NUnit2.dll";
      "Src/AutoFixture.NUnit2/bin/Release/net45/Ploeh.AutoFixture.NUnit2.pdb";
      "Src/AutoFixture.NUnit2/bin/Release/net45/Ploeh.AutoFixture.NUnit2.XML";
      "Src/AutoFixture.NUnit2/bin/Release/net45/nunit.core.interfaces.dll"
      "Src/AutoFixture.NUnit3/bin/Release/net45/Ploeh.AutoFixture.NUnit3.dll";
      "Src/AutoFixture.NUnit3/bin/Release/net45/Ploeh.AutoFixture.NUnit3.pdb";
      "Src/AutoFixture.NUnit3/bin/Release/net45/Ploeh.AutoFixture.NUnit3.XML";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.dll";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.pdb";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.XML";
    ]
    let nuGetPackageScripts = !! "NuGet/*.ps1" ++ "NuGet/*.txt" ++ "NuGet/*.pp" |> List.ofSeq
    let releaseFiles = buildOutput @ nuGetPackageScripts

    releaseFiles
    |> CopyFiles releaseFolder
)

Target "CleanNuGetPackages" (fun _ ->
    CleanDir nuGetOutputFolder
)

Target "NuGetPack" (fun _ ->
    // We have an issue that each ProjectReference is set to 1.0.0 in the produced NuGet package.
    // We apply a workaround for the issue: https://github.com/NuGet/Home/issues/4337.
    build "Restore" "Release" [ "RestorePackagesPath", FullName nuGetRestoreFolder ]

    // Pack most projects using MSBuild and later perform a NuGet pack for leftovers.
    build "Pack" "Release" [ "IncludeSource", "true"
                             "IncludeSymbols", "true"
                             "PackageOutputPath", FullName nuGetOutputFolder
                             "NoBuild", "true" ]

    let version = buildVersion.nugetVersion
    let nuSpecFiles = !! "NuGet/*.nuspec"

    nuSpecFiles
    |> Seq.iter (fun f -> NuGet (fun p -> { p with Version = version
                                                   WorkingDir = releaseFolder
                                                   OutputPath = nuGetOutputFolder
                                                   SymbolPackage = NugetSymbolPackage.Nuspec }) f)
)

let publishPackagesWithSymbols packageFeed symbolFeed accessKey =
    nuGetPackages
    |> Seq.map (fun pkg ->
        let meta = GetMetaDataFromPackageFile pkg
        meta.Id, meta.Version
    )
    |> Seq.iter (fun (id, version) -> NuGetPublish (fun p -> { p with Project = id
                                                                      Version = version
                                                                      OutputPath = nuGetOutputFolder
                                                                      PublishUrl = packageFeed
                                                                      AccessKey = accessKey
                                                                      SymbolPublishUrl = symbolFeed
                                                                      SymbolAccessKey = accessKey
                                                                      WorkingDir = releaseFolder }))

Target "PublishNuGetPublic" (fun _ ->
    let feed = "https://www.nuget.org/api/v2/package"
    let key = getBuildParam "NuGetPublicKey"

    publishPackagesWithSymbols feed "" key
)

Target "PublishNuGetPrivate" (fun _ ->
    let packageFeed = "https://www.myget.org/F/autofixture/api/v2/package"
    let symbolFeed = "https://www.myget.org/F/autofixture/symbols/api/v2/package"
    let key = getBuildParam "NuGetPrivateKey"

    publishPackagesWithSymbols packageFeed symbolFeed key
)

Target "CompleteBuild"   DoNothing
Target "PublishNuGetAll" DoNothing

"CleanVerify"  ==> "CleanAll"
"CleanRelease" ==> "CleanAll"

"CleanReleaseFolder"   ==> "Verify"
"CleanAll"             ==> "Verify"
"RestoreNuGetPackages" ==> "Verify"

"Verify"                             ==> "Build"
"PatchAssemblyVersions"              ==> "Build"
"BuildOnly"                          ==> "Build"
"RestorePatchedAssemblyVersionFiles" ==> "Build"

"BuildOnly" ?=> "RestorePatchedAssemblyVersionFiles"

"BuildOnly" 
    ==> "TestOnly"
    ==> "BuildAndTestOnly"

"Build"    ==> "Test"
"TestOnly" ==> "Test"

"Test" ==> "CopyToReleaseFolder"

"CleanNuGetPackages"  ==> "NuGetPack"
"CopyToReleaseFolder" ==> "NuGetPack"

"NuGetPack" ==> "CompleteBuild"

"NuGetPack" ==> "PublishNuGetPublic"

"NuGetPack" ==> "PublishNuGetPrivate"

"PublishNuGetPublic"  ==> "PublishNuGetAll"
"PublishNuGetPrivate" ==> "PublishNuGetAll"

// ==============================================
// ================== AppVeyor ==================
// ==============================================

// Add helper to identify whether current trigger is PR
type AppVeyorEnvironment with
    static member IsPullRequest = isNotNullOrEmpty AppVeyorEnvironment.PullRequestNumber

type AppVeyorTrigger = SemVerTag | CustomTag | PR | VNextBranch | Unknown
let anAppVeyorTrigger =
    let tag = if AppVeyorEnvironment.RepoTag then Some AppVeyorEnvironment.RepoTagName else None
    let isPR = AppVeyorEnvironment.IsPullRequest
    let branch = if isNotNullOrEmpty AppVeyorEnvironment.RepoBranch then Some AppVeyorEnvironment.RepoBranch else None

    match tag, isPR, branch with
    | Some t, _, _ when "v\d+.*" >** t -> SemVerTag
    | Some _, _, _                     -> CustomTag
    | _, true, _                       -> PR
    // Branch name should be checked after the PR flag, because for PR it's set to the upstream branch name.
    | _, _, Some br when "v\d+" >** br -> VNextBranch
    | _                                -> Unknown

// Print state info at the very beginning
if buildServer = BuildServer.AppVeyor 
   then logfn "[AppVeyor state] Is tag: %b, tag name: '%s', is PR: %b, branch name: '%s', trigger: %A"
              AppVeyorEnvironment.RepoTag 
              AppVeyorEnvironment.RepoTagName 
              AppVeyorEnvironment.IsPullRequest
              AppVeyorEnvironment.RepoBranch
              anAppVeyorTrigger

Target "AppVeyor_SetVNextVersion" (fun _ ->
    // vNext branch has the following name: "vX", where X is the next version
    AppVeyorEnvironment.RepoBranch.Substring(1) 
    |> int
    |> setVNextBranchVersion
)

Target "AppVeyor" (fun _ ->
    //Artifacts might be deployable, so we update build version to find them later by file version
    if not AppVeyorEnvironment.IsPullRequest then UpdateBuildVersion buildVersion.fileVersion
)

"AppVeyor_SetVNextVersion" =?> ("PatchAssemblyVersions", anAppVeyorTrigger = VNextBranch)

// Add logic to resolve action based on current trigger info
dependency "AppVeyor" <| match anAppVeyorTrigger with
                         | SemVerTag                -> "PublishNuGetPublic"
                         | VNextBranch              -> "PublishNuGetPrivate"
                         | PR | CustomTag | Unknown -> "CompleteBuild"


// ========= ENTRY POINT =========
RunTargetOrDefault "CompleteBuild"
