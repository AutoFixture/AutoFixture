#r @"packages/FAKE.Core/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.Testing
open System
open System.Diagnostics;
open System.Text.RegularExpressions

let releaseFolder = "Release"
let nunitToolsFolder = "Packages/NUnit.Runners.2.6.2/tools"
let nuGetOutputFolder = "NuGetPackages"
let solutionsToBuild = !! "Src/*.sln"
let signKeyPath = FullName "Src/AutoFixture.snk"

type GitVersion = { apiVersion:string; nugetVersion:string }
let getGitVersion = 
    let desc = Git.CommandHelper.runSimpleGitCommand "" "describe --tags --long --match=v*"
    // Example for regular: v3.50.2-288-g64fd5c5b, for prerelease: v3.50.2-alpha1-288-g64fd5c5b
    let result = Regex.Match(desc, @"^v(?<maj>\d+)\.(?<min>\d+)\.(?<rev>\d+)(?<pre>-\w+\d*)?-(?<num>\d+)-g(?<sha>[a-z0-9]+)$", RegexOptions.IgnoreCase).Groups
    let getMatch (name:string) = result.[name].Value
    
    let assemblyVer = sprintf "%s.%s.%s" (getMatch "maj") (getMatch "min") (getMatch "rev")    
    let apiVer = sprintf "%s.0" assemblyVer

    let formatCommitsSinceLastTag pattern = 
        match getMatch "num" |> int with
        | 0 -> ""
        | commits -> sprintf pattern commits

    let nugetVer = sprintf "%s%s" assemblyVer (match (getMatch "pre") with "" -> formatCommitsSinceLastTag ".%d" | p -> p + (formatCommitsSinceLastTag ".%04d"))
    
    { apiVersion = apiVer ; nugetVersion = nugetVer }

Target "PatchAssemblyVersions" (fun _ ->
    let version = 
        match getBuildParamOrDefault "Version" "git" with
        | "git"    -> getGitVersion
        | custom -> { apiVersion = custom; nugetVersion = match getBuildParam "NugetVersion" with "" -> custom | v -> v }

    !! "Src/*/Properties/AssemblyInfo.*"
    |> Seq.iter (fun f -> UpdateAttributes f [ Attribute.Version              version.apiVersion
                                               Attribute.FileVersion          version.apiVersion
                                               Attribute.InformationalVersion version.nugetVersion ])
)

let build target configuration =
    solutionsToBuild
    |> Seq.iter (fun s -> build (fun p -> { p with Targets = [target]
                                                   Properties = 
                                                      [
                                                          "Configuration", configuration
                                                          "AssemblyOriginatorKeyFile", signKeyPath
                                                      ] }) s)
    |> ignore

let clean   = build "Clean"
let rebuild = build "Rebuild"

Target "CleanAll"           (fun _ -> ())
Target "CleanVerify"        (fun _ -> clean "Verify")
Target "CleanRelease"       (fun _ -> clean "Release")
Target "CleanReleaseFolder" (fun _ -> CleanDir releaseFolder)

Target "Verify" (fun _ -> rebuild "Verify")

Target "BuildOnly" (fun _ -> rebuild "Release")
Target "TestOnly" (fun _ ->
    let configuration = getBuildParamOrDefault "Configuration" "Release"
    let parallelizeTests = getBuildParamOrDefault "ParallelizeTests" "False" |> Convert.ToBoolean
    let maxParallelThreads = getBuildParamOrDefault "MaxParallelThreads" "0" |> Convert.ToInt32
    let parallelMode = if parallelizeTests then ParallelMode.All else ParallelMode.NoParallelization
    let maxThreads = if maxParallelThreads = 0 then CollectionConcurrencyMode.Default else CollectionConcurrencyMode.MaxThreads(maxParallelThreads)

    let testAssemblies = !! (sprintf "Src/*Test/bin/%s/*Test.dll" configuration)
                         -- (sprintf "Src/AutoFixture.NUnit*.*Test/bin/%s/*Test.dll" configuration)

    testAssemblies
    |> xUnit2 (fun p -> { p with Parallel = parallelMode
                                 MaxThreads = maxThreads })

    let nunit2TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit2.*Test/bin/%s/*Test.dll" configuration)

    nunit2TestAssemblies
    |> NUnit (fun p -> { p with StopOnError = false
                                OutputFile = "NUnit2TestResult.xml" })

    let nunit3TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit3.UnitTest/bin/%s/Ploeh.AutoFixture.NUnit3.UnitTest.dll" configuration)

    nunit3TestAssemblies
    |> NUnit3 (fun p -> { p with StopOnError = false
                                 ResultSpecs = ["NUnit3TestResult.xml;format=nunit2"] })
)

Target "BuildAndTestOnly" (fun _ -> ())
Target "Build" (fun _ -> ())
Target "Test"  (fun _ -> ())

Target "CopyToReleaseFolder" (fun _ ->
    let buildOutput = [
      "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.dll";
      "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.pdb";
      "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.XML";
      "Src/SemanticComparison/bin/Release/Ploeh.SemanticComparison.dll";
      "Src/SemanticComparison/bin/Release/Ploeh.SemanticComparison.pdb";
      "Src/SemanticComparison/bin/Release/Ploeh.SemanticComparison.XML";
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
    let version = FileVersionInfo.GetVersionInfo("Src/AutoFixture/bin/Release/Ploeh.AutoFixture.dll").ProductVersion

    let nuSpecFiles = !! "NuGet/*.nuspec"

    nuSpecFiles
    |> Seq.iter (fun f -> NuGet (fun p -> { p with Version = version
                                                   WorkingDir = releaseFolder
                                                   OutputPath = nuGetOutputFolder
                                                   SymbolPackage = NugetSymbolPackage.Nuspec }) f)
)

let publishPackagesToNuGet apiFeed symbolFeed nugetKey =
    let packages = !! (sprintf "%s/*.nupkg" nuGetOutputFolder)     

    packages
    |> Seq.map (fun p ->
        let isSymbolPackage = p.EndsWith "symbols.nupkg"
        let feed =
            match isSymbolPackage with
            | true -> symbolFeed
            | false -> apiFeed

        let meta = GetMetaDataFromPackageFile p
        let version = 
            match isSymbolPackage with
            | true -> sprintf "%s.symbols" meta.Version
            | false -> meta.Version

        (meta.Id, version, feed))
    |> Seq.iter (fun (id, version, feed) -> NuGetPublish (fun p -> { p with PublishUrl = feed
                                                                            AccessKey = nugetKey
                                                                            OutputPath = nuGetOutputFolder
                                                                            Project = id
                                                                            Version = version }))    

Target "PublishNuGetPreRelease" (fun _ -> publishPackagesToNuGet 
                                            "https://www.myget.org/F/autofixture/api/v2/package" 
                                            "https://www.myget.org/F/autofixture/symbols/api/v2/package"
                                            (getBuildParam "NuGetPreReleaseKey"))

Target "PublishNuGetRelease" (fun _ -> publishPackagesToNuGet
                                            "https://www.nuget.org/api/v2/package"
                                            "https://nuget.smbsrc.net/"
                                            (getBuildParam "NuGetReleaseKey"))

Target "CompleteBuild" (fun _ -> ())
Target "PublishNuGetAll" (fun _ -> ()) 

"CleanVerify"  ==> "CleanAll"
"CleanRelease" ==> "CleanAll"

"CleanReleaseFolder"    ==> "Verify"
"CleanAll"              ==> "Verify"

"Verify"                ==> "Build"
"PatchAssemblyVersions" ==> "Build"
"BuildOnly"             ==> "Build"

"Build"    ==> "Test"
"TestOnly" ==> "Test"

"BuildOnly" 
    ==> "TestOnly"
    ==> "BuildAndTestOnly"

"Test" ==> "CopyToReleaseFolder"

"CleanNuGetPackages"  ==> "NuGetPack"
"CopyToReleaseFolder" ==> "NuGetPack"

"NuGetPack" ==> "CompleteBuild"

"NuGetPack" ==> "PublishNuGetRelease"
"NuGetPack" ==> "PublishNuGetPreRelease"

"PublishNuGetRelease"    ==> "PublishNuGetAll"
"PublishNuGetPreRelease" ==> "PublishNuGetAll"



RunTargetOrDefault "CompleteBuild"
