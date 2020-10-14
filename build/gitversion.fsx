#load "../.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Newtonsoft.Json

let getVersion () =
    let proc = Command.RawCommand("dotnet-gitversion", Arguments.Empty)
                 |> CreateProcess.fromCommand
                 |> CreateProcess.withToolType (ToolType.CreateLocalTool())
                 |> CreateProcess.redirectOutput
                 |> Proc.run

    if proc.ExitCode <> 0 then
        failwithf "GitVersion failed with exit code %i and message %s" proc.ExitCode proc.Result.Output

    proc.Result.Output |> fun x -> JsonConvert.DeserializeObject<Fake.Tools.GitVersion.GitVersionProperties>(x)
