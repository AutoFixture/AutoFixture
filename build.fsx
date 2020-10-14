#r "paket:
nuget Fake.BuildServer.AppVeyor
nuget Fake.Core.Environment
nuget Fake.Core.Xml
nuget Fake.DotNet
nuget Fake.DotNet.Cli
nuget Fake.DotNet.MsBuild
nuget Fake.DotNet.NuGet
nuget Fake.DotNet.Testing.NUnit
nuget Fake.IO.FileSystem
nuget Fake.IO.Zip
nuget Fake.Runtime
nuget Fake.Tools.Git
nuget Fake.Tools.GitVersion
nuget Fake.Testing.Common
nuget Fake.Core.Target
"

#load ".fake/build.fsx/intellisense.fsx"
#load "./build/gitversion.fsx"
#load "./build/msbuild.fsx"
#load "./build/appveyor.fsx"
#load "./build/nuget.fsx"

open System
open System.Convert
open Fake.Core
open Fake.BuildServer
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.Tools
open Gitversion
open Msbuild
open Appveyor
open Nuget

BuildServer.install [ AppVeyor.Installer ]

let versionInfo = getVersion()
let isCIBuild = BuildServer.buildServer <> LocalBuild
let artifactsDirectory = Environment.environVarOrDefault "BUILD_DIR" "artifacts"
                         |> Path.getFullName
let resultsDirectory = artifactsDirectory </> "testresults"
                       |> Path.getFullName
let nugetOutputFolder = artifactsDirectory </> "packages"
let testResultsFolder = artifactsDirectory </> "testresults"
let verbosity = Environment.environVarOrDefault "BUILD_VERBOSITY" ""
                |> String.toLower
                |> mapVerbosity
let configuration =
  DotNet.BuildConfiguration.fromEnvironVarOrDefault
    "BUILD_CONFIGURATION"
    DotNet.BuildConfiguration.Release

let getFeed (versionInfo:GitVersion.GitVersionProperties) =
  match versionInfo.BranchName, versionInfo.PreReleaseLabel with
  | "master", _   -> getNuGetFeed ()
  | _, "preview"  -> getMyGetFeed ()
  | _, _          -> None

let setMsBuildParams parameters:MSBuild.CliArguments =
  let isCIBuildStr = isCIBuild |> ToString
  parameters
  |> setVerbosity (verbosity |> Some)
  |> setVersionInfo versionInfo
  |> setSourceLink isCIBuildStr
  |> setCIBuild isCIBuildStr

let setBuildOptions options:DotNet.BuildOptions =
  {options with
    NoLogo = true
    MSBuildParams = options.MSBuildParams
      |> setMsBuildParams
    Configuration = configuration}

let setVerifyOptions options:DotNet.BuildOptions =
  {options with
    NoLogo = true
    MSBuildParams = options.MSBuildParams
      |> setMsBuildParams
    Configuration = DotNet.BuildConfiguration.Custom "Verify"}

let setTestOptions options:DotNet.TestOptions =
  {options with
    NoLogo = true
    NoRestore = true
    NoBuild = true
    Configuration = configuration
    ResultsDirectory = resultsDirectory |> Some
    Logger = "trx" |> Some
    MSBuildParams = options.MSBuildParams
      |> setVerbosity (verbosity |> Some)
    RunSettingsArguments = Some "-- RunConfiguration.NoAutoReporters=true"}

let setPackOptions options:DotNet.PackOptions =
  {options with
    NoLogo = true
    NoRestore = true
    // NoBuild = true // Not working due to a bug in the pack tool https://github.com/NuGet/Home/issues/3891
    OutputPath = nugetOutputFolder |> Some
    MSBuildParams = options.MSBuildParams
      |> setMsBuildParams
    Configuration = configuration}

let setNugetPushOptions feed options:DotNet.NuGetPushOptions =
  TraceSecrets.register "<api key>" feed.ApiKey
  {options with
    PushParams =
      {options.PushParams with
        Source = feed.Source |> Some
        ApiKey = feed.ApiKey |> Some}}

let setBuildParameters parameters:AppVeyor.UpdateBuildParams =
  parameters |> setVersion versionInfo.SemVer

Target.initEnvironment ()

Target.create "Clean" (fun _ ->
  !! "src/**/bin"
  ++ "src/**/obj"
  ++ "artifacts"
  ++ "packages"
  ++ "testresults"
  |> Shell.cleanDirs
)

Target.create "Build" (fun _ ->
  !! "src/All.sln"
  |> Seq.iter (DotNet.build setBuildOptions)
)

Target.create "Verify" (fun _ ->
  !! "src/All.sln"
  |> Seq.iter (DotNet.build setVerifyOptions)
)

Target.create "Test" (fun _ ->
  !! "src/All.sln"
  |> Seq.iter (DotNet.test setTestOptions)
)

Target.create "Pack" (fun _ ->
  !! "src/All.sln"
  |> Seq.iter (DotNet.pack setPackOptions)
)

Target.create "Publish" (fun _ ->
  match versionInfo |> getFeed with
  | Some feed  -> !! (nugetOutputFolder </> "*.nupkg")
               |> Seq.iter (DotNet.nugetPush (setNugetPushOptions feed))
  | None    -> ignore ()
)

Target.create "Test:Publish:Results" (fun _ ->
  let shuffle xs = xs |> Seq.sortBy (fun _ -> Guid())

  !!"*.trx"
  |> GlobbingPattern.setBaseDir testResultsFolder
  |> Seq.map (fun file -> (file, ImportData.Mstest))
  |> shuffle
  |> Seq.map (fun (file, format) -> async { Trace.publish format file })
  |> Async.Parallel
  |> Async.RunSynchronously
  |> ignore
)

Target.create "AppVeyor:Setup" (fun _ -> 
  AppVeyor.updateBuild setBuildParameters
)

Target.create "AppVeyor" ignore

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "Pack"
  ==> "All"

"Clean"
  ==> "Verify"

"Pack"
  ==> "Publish"

"Test"
  ==> "Test:Publish:Results"

"AppVeyor:Setup"
  ==> "Verify"
  ==> "Test:Publish:Results"
  ==> "Publish"
  ==> "AppVeyor"


Target.runOrDefault "All"
