#r @"tools/FAKE.Core/tools/FakeLib.dll"

open Fake
open Fake.AppVeyor
open Fake.Testing
open System
open System.Diagnostics;
open System.Text.RegularExpressions

let releaseFolder = "Release"
let nunitToolsFolder = "Packages/NUnit.Runners.2.6.2/tools"
let nuGetOutputFolder = "NuGetPackages"
let nuGetPackages = !! (nuGetOutputFolder @@ "*.nupkg" )
                    // Skip symbol packages because NuGet publish symbols automatically when package is published
                    -- (nuGetOutputFolder @@ "*.symbols.nupkg")
                    // Currently AutoFakeItEasy2 has been deprecated and is not being published to the feeds.
                    -- (nuGetOutputFolder @@ "AutoFixture.AutoFakeItEasy2.*" )
let signKeyPath = FullName "Src/AutoFixture.snk"
let solutionsToBuild = !! "Src/All.sln"

type BuildVersionInfo = { assemblyVersion:string; fileVersion:string; infoVersion:string; nugetVersion:string }
let calculateVersionFromGit buildNumber =
    // Example of output for a release tag: v3.50.2-288-g64fd5c5b, for a prerelease tag: v3.50.2-alpha1-288-g64fd5c5b
    let desc = Git.CommandHelper.runSimpleGitCommand "" "describe --tags --long --match=v*"

    let result = Regex.Match(desc,
                             @"^v(?<maj>\d+)\.(?<min>\d+)\.(?<rev>\d+)(?<pre>-\w+\d*)?-(?<num>\d+)-g(?<sha>[a-z0-9]+)$",
                             RegexOptions.IgnoreCase)
                      .Groups
    let getMatch (name:string) = result.[name].Value

    let major, minor, revision, preReleaseSuffix, commitsNum, sha =
        getMatch "maj" |> int, getMatch "min" |> int, getMatch "rev" |> int, getMatch "pre", getMatch "num" |> int, getMatch "sha"

    
    let assemblyVersion = sprintf "%d.%d.%d.0" major minor revision
    let fileVersion = sprintf "%d.%d.%d.%d" major minor revision buildNumber
    
    // If number of commits since last tag is greater than zero, we append another identifier with number of commits.
    // The produced version is larger than the last tag version.
    // If we are on a tag, we use version specified modification.
    // Examples of output: 3.50.2.1, 3.50.2.215, 3.50.1-rc1.3, 3.50.1-rc3.35
    let nugetVersion = match commitsNum with
                       | 0 -> sprintf "%d.%d.%d%s" major minor revision preReleaseSuffix
                       | _ -> sprintf "%d.%d.%d%s.%d" major minor revision preReleaseSuffix commitsNum

    let infoVersion = match commitsNum with
                      | 0 -> nugetVersion
                      | _ -> sprintf "%s-%s" nugetVersion sha

    { assemblyVersion=assemblyVersion; fileVersion=fileVersion; infoVersion=infoVersion; nugetVersion=nugetVersion }

// Calculate version that should be used for the build. Define globally as data might be required by multiple targets.
// Please never name the build parameter with version as "Version" - it might be consumed by the MSBuild, override 
// the defined properties and break some tasks (e.g. NuGet restore).
let buildVersion = match getBuildParamOrDefault "BuildVersion" "git" with
                   | "git"       -> calculateVersionFromGit (getBuildParamOrDefault "BuildNumber" "0" |> int)
                   | assemblyVer -> { assemblyVersion = assemblyVer
                                      fileVersion = getBuildParamOrDefault "BuildFileVersion" assemblyVer
                                      infoVersion = getBuildParamOrDefault "BuildInfoVersion" assemblyVer
                                      nugetVersion = getBuildParamOrDefault "BuildNugetVersion" assemblyVer }



Target "PatchAssemblyVersions" (fun _ ->
    printfn 
        "Patching assembly versions. Assembly version: %s, File version: %s, Informational version: %s" 
        buildVersion.assemblyVersion
        buildVersion.fileVersion
        buildVersion.infoVersion

    let filesToPatch = !! "Src/*/Properties/AssemblyInfo.*"
    ReplaceAssemblyInfoVersionsBulk filesToPatch 
                                    (fun f -> { f with AssemblyVersion              = buildVersion.assemblyVersion
                                                       AssemblyFileVersion          = buildVersion.fileVersion
                                                       AssemblyInformationalVersion = buildVersion.infoVersion })
)

let build target configuration =
    let properties = [ "Configuration", configuration
                       "AssemblyOriginatorKeyFile", signKeyPath
                       "AssemblyVersion", buildVersion.assemblyVersion
                       "FileVersion", buildVersion.fileVersion
                       "InformationalVersion", buildVersion.infoVersion ]

    solutionsToBuild
    |> MSBuild "" target properties
    |> ignore

let clean   = build "Clean"
let rebuild = build "Rebuild"

Target "CleanAll"           DoNothing 
Target "CleanVerify"        (fun _ -> clean "Verify")
Target "CleanRelease"       (fun _ -> clean "Release")
Target "CleanReleaseFolder" (fun _ -> CleanDir releaseFolder)

Target "RestoreNuGetPackages" (fun _ ->
    solutionsToBuild
    |> Seq.iter (RestoreMSSolutionPackages (fun p -> { p with OutputPath = "Packages" }))
)

Target "Verify" (fun _ -> rebuild "Verify")

Target "BuildOnly" (fun _ -> rebuild "Release")
Target "TestOnly" (fun _ ->
    let configuration = getBuildParamOrDefault "Configuration" "Release"
    let parallelizeTests = getBuildParamOrDefault "ParallelizeTests" "False" |> Convert.ToBoolean
    let maxParallelThreads = getBuildParamOrDefault "MaxParallelThreads" "0" |> Convert.ToInt32
    let parallelMode = if parallelizeTests then ParallelMode.All else ParallelMode.NoParallelization
    let maxThreads = if maxParallelThreads = 0 then CollectionConcurrencyMode.Default else CollectionConcurrencyMode.MaxThreads(maxParallelThreads)

    let testAssemblies = !! (sprintf "Src/*Test/bin/%s/**/*Test.dll" configuration)
                         -- (sprintf "Src/AutoFixture.NUnit*.*Test/bin/%s/**/*Test.dll" configuration)

    testAssemblies
    |> xUnit2 (fun p -> { p with Parallel = parallelMode
                                 MaxThreads = maxThreads })

    let nunit2TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit2.*Test/bin/%s/*Test.dll" configuration)

    nunit2TestAssemblies
    |> NUnitSequential.NUnit (fun p -> { p with StopOnError = false
                                                OutputFile = "NUnit2TestResult.xml" })

    let nunit3TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit3.UnitTest/bin/%s/Ploeh.AutoFixture.NUnit3.UnitTest.dll" configuration)

    nunit3TestAssemblies
    |> Testing.NUnit3.NUnit3 (fun p -> { p with StopOnError = false
                                                ResultSpecs = ["NUnit3TestResult.xml;format=nunit2"] })
)

Target "BuildAndTestOnly" DoNothing
Target "Build" DoNothing
Target "Test"  DoNothing

Target "CopyToReleaseFolder" (fun _ ->
    let buildOutput = [
      "Src/AutoFixture/bin/Release/net45/Ploeh.AutoFixture.dll";
      "Src/AutoFixture/bin/Release/net45/Ploeh.AutoFixture.pdb";
      "Src/AutoFixture/bin/Release/net45/Ploeh.AutoFixture.XML";
      "Src/SemanticComparison/bin/Release/net40/Ploeh.SemanticComparison.dll";
      "Src/SemanticComparison/bin/Release/net40/Ploeh.SemanticComparison.pdb";
      "Src/SemanticComparison/bin/Release/net40/Ploeh.SemanticComparison.XML";
      "Src/AutoMoq/bin/Release/Ploeh.AutoFixture.AutoMoq.dll";
      "Src/AutoMoq/bin/Release/Ploeh.AutoFixture.AutoMoq.pdb";
      "Src/AutoMoq/bin/Release/Ploeh.AutoFixture.AutoMoq.XML";
      "Src/AutoRhinoMock/bin/Release/Ploeh.AutoFixture.AutoRhinoMock.dll";
      "Src/AutoRhinoMock/bin/Release/Ploeh.AutoFixture.AutoRhinoMock.pdb";
      "Src/AutoRhinoMock/bin/Release/Ploeh.AutoFixture.AutoRhinoMock.XML";
      "Src/AutoFakeItEasy/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy.dll";
      "Src/AutoFakeItEasy/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy.pdb";
      "Src/AutoFakeItEasy/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy.XML";
      "Src/AutoFakeItEasy2/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy2.dll";
      "Src/AutoFakeItEasy2/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy2.pdb";
      "Src/AutoFakeItEasy2/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy2.XML";
      "Src/AutoNSubstitute/bin/Release/Ploeh.AutoFixture.AutoNSubstitute.dll";
      "Src/AutoNSubstitute/bin/Release/Ploeh.AutoFixture.AutoNSubstitute.pdb";
      "Src/AutoNSubstitute/bin/Release/Ploeh.AutoFixture.AutoNSubstitute.XML";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.dll";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.pdb";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.XML";
      "Src/AutoFixture.xUnit.net/bin/Release/Ploeh.AutoFixture.Xunit.dll";
      "Src/AutoFixture.xUnit.net/bin/Release/Ploeh.AutoFixture.Xunit.pdb";
      "Src/AutoFixture.xUnit.net/bin/Release/Ploeh.AutoFixture.Xunit.XML";
      "Src/AutoFixture.xUnit.net2/bin/Release/Ploeh.AutoFixture.Xunit2.dll";
      "Src/AutoFixture.xUnit.net2/bin/Release/Ploeh.AutoFixture.Xunit2.pdb";
      "Src/AutoFixture.xUnit.net2/bin/Release/Ploeh.AutoFixture.Xunit2.XML";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.dll";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.pdb";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.XML";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.Addins.dll";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.Addins.pdb";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.Addins.XML";
      "Src/AutoFixture.NUnit3/bin/Release/Ploeh.AutoFixture.NUnit3.dll";
      "Src/AutoFixture.NUnit3/bin/Release/Ploeh.AutoFixture.NUnit3.pdb";
      "Src/AutoFixture.NUnit3/bin/Release/Ploeh.AutoFixture.NUnit3.XML";
      "Src/Idioms/bin/Release/Ploeh.AutoFixture.Idioms.dll";
      "Src/Idioms/bin/Release/Ploeh.AutoFixture.Idioms.pdb";
      "Src/Idioms/bin/Release/Ploeh.AutoFixture.Idioms.XML";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.dll";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.pdb";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.XML";
      nunitToolsFolder @@ "lib/nunit.core.interfaces.dll"
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

"Verify"                ==> "Build"
"PatchAssemblyVersions" ==> "Build"
"BuildOnly"             ==> "Build"

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

type AppVeyorTrigger = SemVerTag | CustomTag | PR | Unknown
let anAppVeyorTrigger =
    let tag = if AppVeyorEnvironment.RepoTag then Some AppVeyorEnvironment.RepoTagName else None
    let isPR = AppVeyorEnvironment.IsPullRequest

    match tag, isPR with
    | Some t, _ when "v\d+.*" >** t -> SemVerTag
    | Some _, _                     -> CustomTag
    | _, true                       -> PR
    | _                             -> Unknown

// Print state info at the very beginning
if buildServer = BuildServer.AppVeyor 
   then logfn "[AppVeyor state] Is tag: %b, tag name: '%s', is PR: %b, branch name: '%s', trigger: %A"
              AppVeyorEnvironment.RepoTag 
              AppVeyorEnvironment.RepoTagName 
              AppVeyorEnvironment.IsPullRequest
              AppVeyorEnvironment.RepoBranch
              anAppVeyorTrigger

Target "AppVeyor" (fun _ ->
    //Artifacts might be deployable, so we update build version to find them later by file version
    if not AppVeyorEnvironment.IsPullRequest then UpdateBuildVersion buildVersion.fileVersion
)

// Add logic to resolve action based on current trigger info
dependency "AppVeyor" <| match anAppVeyorTrigger with
                         | SemVerTag                -> "PublishNuGetPublic"
                         | PR | CustomTag | Unknown -> "CompleteBuild"


// ========= ENTRY POINT =========
RunTargetOrDefault "CompleteBuild"
