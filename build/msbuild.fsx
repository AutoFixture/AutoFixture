#load "../.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.Tools

let mapVerbosity verbosity =
    match verbosity with
    | "quiet"      | "q"    -> Quiet
    | "minimal"    | "m"    -> Minimal
    | "normal"     | "n"    -> Normal
    | "detailed"   | "d"    -> Detailed
    | "diagnostic" | "diag" -> Diagnostic
    | _ -> Minimal

let setCIBuild enabled parameters:MSBuild.CliArguments =
    let ciBuildParams = [ "ContinuousIntegrationBuild", enabled ]
    {parameters
        with Properties = parameters.Properties @ ciBuildParams}

let setSourceLink enabled parameters:MSBuild.CliArguments =
    let sourceLinkParams = [ "EnableSourceLink", enabled ]
    {parameters
        with Properties = parameters.Properties @ sourceLinkParams}

let setVersionInfo (versionInfo:GitVersion.GitVersionProperties) (parameters:MSBuild.CliArguments) =
    let versionParams = [
        "AssemblyVersion", versionInfo.AssemblySemVer
        "FileVersion", versionInfo.AssemblySemVer
        "InformationalVersion", versionInfo.InformationalVersion
        "PackageVersion", versionInfo.NuGetVersionV2
        "CommitHash", versionInfo.Sha
        ]
    {parameters with
        Properties = parameters.Properties @ versionParams}

let setVerbosity verbosity p:MSBuild.CliArguments =
    { p with Verbosity = verbosity }

let isPreviewVersion (versionInfo:GitVersion.GitVersionProperties) =
    versionInfo.PreReleaseLabel
    |> String.isNotNullOrEmpty
